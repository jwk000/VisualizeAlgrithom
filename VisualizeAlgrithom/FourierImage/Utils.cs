using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace fourier2
{
    public class Triangle
    {
        public int v0;
        public int v1;
        public int v2;
    }
    public class Edge
    {
        public int a;
        public int b;
        public float w;

        public Edge(int va, int vb, float w)
        {
            a = va;
            b = vb;
            this.w = w;
        }

        public int hash()
        {
            if (b < a) return (b << 16) | a;
            else return (a << 16) | b;
        }
    }
    public class Circle
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }
    }
    public class Delaunay
    {
        List<Vector2> vertices = new List<Vector2>();
        List<Triangle> triangles = new List<Triangle>();
        List<Edge> edges = new List<Edge>();

        public int HandlingVertex = 0;//正在处理的顶点
        public Triangle HandlingTriangle = null;
        public Circle HandlingCircle = null;
        const int BoxSize = 500;


        public List<Edge>  Triangulate(List<Vector2> vertices)
        {
            this.vertices = vertices;
            float minX = vertices[0].X;
            float maxX = vertices[0].X;
            float minY = vertices[0].Y;
            float maxY = vertices[0].Y;

            for (int i = 1; i < vertices.Count; i++)
            {
                if (vertices[i].X < minX) { minX = vertices[i].X; }
                if (vertices[i].X > maxX) { maxX = vertices[i].X; }
                if (vertices[i].Y < minY) { minY = vertices[i].Y; }
                if (vertices[i].Y > maxY) { maxY = vertices[i].Y; }
            }

            Vector2 A = new Vector2(minX - BoxSize, minY - BoxSize);
            Vector2 B = new Vector2(minX - BoxSize, maxY + BoxSize);
            Vector2 C = new Vector2(maxX + BoxSize, maxY + BoxSize);
            Vector2 D = new Vector2(maxX + BoxSize, minY - BoxSize);
            vertices.Add(A); int ia = vertices.Count - 1;
            vertices.Add(B); int ib = vertices.Count - 1;
            vertices.Add(C); int ic = vertices.Count - 1;
            vertices.Add(D); int id = vertices.Count - 1;
            triangles.Add(new Triangle() { v0 = ia, v1 = ib, v2 = ic });
            triangles.Add(new Triangle() { v0 = ia, v1 = ic, v2 = id });

            for (int i = 0; i < vertices.Count - 4; i++)
            {
                Vector2 p = vertices[i];
                Triangle t = null;
                foreach (var tr in triangles)
                {
                    if (IsInTriangle(p, tr))
                    {
                        t = tr;
                        break;
                    }
                }
                if (t == null)
                {
                    //TODO 如果p在三角形边上会插入失败，先跳过，后面处理

                    continue;
                }
                var t1 = new Triangle() { v0 = i, v1 = t.v0, v2 = t.v1 };
                var t2 = new Triangle() { v0 = i, v1 = t.v1, v2 = t.v2 };
                var t3 = new Triangle() { v0 = i, v1 = t.v2, v2 = t.v0 };
                triangles.Remove(t);
                triangles.Add(t1);
                triangles.Add(t2);
                triangles.Add(t3);

                AdjustTriangle(t1);
                AdjustTriangle(t2);
                AdjustTriangle(t3);
            }

            CreateEdges();

            return edges;
        }

        void CreateEdges()
        {
            edges.Clear();
            //丢弃辅助点和包含辅助点的三角形
            //但是不能丢掉不包含辅助点的边
            vertices = vertices.Take(vertices.Count - 4).ToList();
            for (int i = triangles.Count - 1; i >= 0; i--)
            {
                var t = triangles[i];
                if (t.v0 < vertices.Count && t.v1 < vertices.Count)
                {
                    float w = (vertices[t.v0] - vertices[t.v1]).LengthSquared();
                    var ed = new Edge(t.v0, t.v1, w);
                    if (0 == edges.Count(e => e.hash() == ed.hash()))
                    {
                        edges.Add(ed);
                    }
                }
                if (t.v1 < vertices.Count && t.v2 < vertices.Count)
                {
                    float w = (vertices[t.v2] - vertices[t.v1]).LengthSquared();
                    var ed = new Edge(t.v1, t.v2, w);
                    if (0 == edges.Count(e => e.hash() == ed.hash()))
                    {
                        edges.Add(ed);
                    }
                }
                if (t.v0 < vertices.Count && t.v2 < vertices.Count)
                {
                    float w = (vertices[t.v0] - vertices[t.v2]).LengthSquared();
                    var ed = new Edge(t.v0, t.v2, w);
                    if (0 == edges.Count(e => e.hash() == ed.hash()))
                    {
                        edges.Add(ed);
                    }
                }
            }

        }

        void AdjustTriangle(Triangle t)
        {
            HandlingCircle = null;
            HandlingTriangle = t;

            //共边三角形
            var r = GetTriangleWithEdge(t.v1, t.v2, t);
            if (r != null)
            {
                int iv = r.v0 + r.v1 + r.v2 - t.v2 - t.v1;
                Vector2 v = vertices[iv];
                if (IsInCircle(v, t))
                {
                    triangles.Remove(t);
                    triangles.Remove(r);
                    triangles.Add(new Triangle() { v0 = t.v0, v1 = t.v1, v2 = iv });
                    triangles.Add(new Triangle() { v0 = t.v0, v1 = iv, v2 = t.v2 });
                }
            }
        }

        Triangle GetTriangleWithEdge(int ia, int ib, Triangle except = null)
        {
            foreach (var t in triangles)
            {
                if (t != except)
                {
                    if (t.v0 == ia || t.v1 == ia || t.v2 == ia)
                    {
                        if (t.v0 == ib || t.v1 == ib || t.v2 == ib)
                        {
                            return t;
                        }
                    }
                }
            }
            return null;
        }

        bool IsInTriangle(Vector2 p, Triangle t)
        {
            Vector2 v0 = vertices[t.v0];
            Vector2 v1 = vertices[t.v1];
            Vector2 v2 = vertices[t.v2];

            Vector3 p01 = Vector3.Cross(new Vector3(p - v0, 0), new Vector3(v1 - v0, 0));
            Vector3 p12 = Vector3.Cross(new Vector3(p - v1, 0), new Vector3(v2 - v1, 0));
            Vector3 p20 = Vector3.Cross(new Vector3(p - v2, 0), new Vector3(v0 - v2, 0));

            if (p01.Z < 0 && p12.Z < 0 && p20.Z < 0)
            {
                return true;
            }
            if (p01.Z > 0 && p12.Z > 0 && p20.Z > 0)
            {
                return true;
            }
            return false;
        }

        bool IsInCircle(Vector2 p, Triangle t)
        {
            Vector2 A = vertices[t.v0];
            Vector2 B = vertices[t.v1];
            Vector2 C = vertices[t.v2];
            Circle circle = Circumcircle(A, B, C);

            float dd = (p - circle.Center).Length();

            HandlingCircle = circle;

            return dd < circle.Radius;
        }

        Circle Circumcircle(Vector2 A, Vector2 B, Vector2 C)
        {
            float D = 2 * (A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y));

            float Ux = ((A.X * A.X + A.Y * A.Y) * (B.Y - C.Y) +
                         (B.X * B.X + B.Y * B.Y) * (C.Y - A.Y) +
                         (C.X * C.X + C.Y * C.Y) * (A.Y - B.Y)) / D;

            float Uy = ((A.X * A.X + A.Y * A.Y) * (C.X - B.X) +
                         (B.X * B.X + B.Y * B.Y) * (A.X - C.X) +
                         (C.X * C.X + C.Y * C.Y) * (B.X - A.X)) / D;

            float radius = MathF.Sqrt((Ux - A.X) * (Ux - A.X) + (Uy - A.Y) * (Uy - A.Y));

            return new Circle { Center = new Vector2(Ux, Uy), Radius = radius };
        }


    }

    class SMTNode
    {
        public int index;//顶点索引
        public List<int> nodes = new List<int>();//邻接顶点
        
        public SMTNode(int i)
        {
            index = i;
        }
    }


}
