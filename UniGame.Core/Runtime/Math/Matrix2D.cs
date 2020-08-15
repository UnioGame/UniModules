namespace UniModules.UniGame.Core.Runtime.Math
{
    using System;
    using UnityEngine;

    public struct Matrix2D
    {
        private static readonly Matrix2D ZeroMatrix = new Matrix2D(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0));
        private static readonly Matrix2D IdentityMatrix = new Matrix2D(new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0));
        
        public float m00;
        public float m10;
        public float m01;
        public float m11;
        public float m02;
        public float m12;
        
        public static Matrix2D Zero => ZeroMatrix;
        public static Matrix2D Identity => IdentityMatrix;

        public float this[int row, int column] {
            get => this[row + column * 2];
            set => this[row + column * 2] = value;
        }

        public float this[int index] {
            get {
                switch (index) {
                    case 0: return m00;
                    case 1: return m10;
                    case 2: return m01;
                    case 3: return m11;
                    case 4: return m02;
                    case 5: return m12;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
            set {
                switch (index) {
                    case 0: m00 = value; break;
                    case 1: m10 = value; break;
                    case 2: m01 = value; break;
                    case 3: m11 = value; break;
                    case 4: m02 = value; break;
                    case 5: m12 = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        public Matrix2D(Vector2 column0, Vector2 column1, Vector2 column2)
        {
            m00 = column0.x;
            m01 = column1.x;
            m02 = column2.x;
            m10 = column0.y;
            m11 = column1.y;
            m12 = column2.y;
        }

        public Vector2 GetColumn(int index)
        {
            switch (index) {
                case 0: return new Vector2(m00, m10);
                case 1: return new Vector2(m01, m11);
                case 2: return new Vector2(m02, m12);
                default:
                    throw new IndexOutOfRangeException("Invalid column index!");
            }
        }

        public Vector3 GetRow(int index)
        {
            switch (index) {
                case 0: return new Vector3(m00, m01, m02);
                case 1: return new Vector3(m10, m11, m12);
                default:
                    throw new IndexOutOfRangeException("Invalid row index!");
            }
        }
        
        public void SetColumn(int index, Vector2 column)
        {
            this[0, index] = column.x;
            this[1, index] = column.y;
        }
        
        public void SetRow(int index, Vector3 row)
        {
            this[index, 0] = row.x;
            this[index, 1] = row.y;
            this[index, 2] = row.z;
        }
        
        public Vector2 MultiplyPoint(Vector2 point)
        {
            Vector2 res;
            res.x = m00 * point.x + m01 * point.y + m02;
            res.y = m10 * point.x + m11 * point.y + m12;
            return res;
        }
        
        public Vector2 MultiplyVector(Vector2 vector)
        {
            Vector2 res;
            res.x = m00 * vector.x + m01 * vector.y;
            res.y = m10 * vector.x + m11 * vector.y;
            return res;
        }
        
        public Matrix2D Inverse()
        {
            var invMat = new Matrix2D();

            var det = this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            if (Mathf.Approximately(0.0f, det))
                return Zero;

            var invDet = 1.0F / det;

            invMat[0, 0] = this[1, 1] * invDet;
            invMat[0, 1] = -this[0, 1] * invDet;
            invMat[1, 0] = -this[1, 0] * invDet;
            invMat[1, 1] = this[0, 0] * invDet;

            invMat[0, 2] = -(this[0, 2] * invMat[0, 0] + this[1, 2] * invMat[0, 1]);
            invMat[1, 2] = -(this[0, 2] * invMat[1, 0] + this[1, 2] * invMat[1, 1]);

            return invMat;
        }
        
        public static Matrix2D Scale(Vector2 vector)
        {
            Matrix2D m;
            m.m00 = vector.x; m.m01 = 0F; m.m02 = 0F;
            m.m10 = 0F; m.m11 = vector.y; m.m12 = 0F;
            return m;
        }
        
        public static Matrix2D Translate(Vector2 vector)
        {
            Matrix2D m;
            m.m00 = 1F; m.m01 = 0F; m.m02 = vector.x;
            m.m10 = 0F; m.m11 = 1F; m.m12 = vector.y;
            return m;
        }
        
        public static Matrix2D RotateRH(float angleRadians)
        {
            return RotateLH(-angleRadians);
        }
        
        public static Matrix2D RotateLH(float angleRadians)
        {
            var s = Mathf.Sin(angleRadians);
            var c = Mathf.Cos(angleRadians);

            Matrix2D m;
            m.m00 = c; m.m10 = -s;
            m.m01 = s; m.m11 = c;
            m.m02 = 0.0F; m.m12 = 0.0F;
            return m;
        }

        public override int GetHashCode()
        {
            return GetColumn(0).GetHashCode() ^ (GetColumn(1).GetHashCode() << 2) ^ (GetColumn(2).GetHashCode() >> 2);
        }
        
        public override bool Equals(object other)
        {
            if (!(other is Matrix2D)) 
                return false;

            var rhs = (Matrix2D)other;
            return GetColumn(0).Equals(rhs.GetColumn(0))
                   && GetColumn(1).Equals(rhs.GetColumn(1))
                   && GetColumn(2).Equals(rhs.GetColumn(2));
        }
        
        public static Matrix2D operator*(Matrix2D lhs, Matrix2D rhs)
        {
            Matrix2D res;
            res.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10;
            res.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11;
            res.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02;

            res.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10;
            res.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11;
            res.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12;

            return res;
        }
        
        public static bool operator==(Matrix2D lhs, Matrix2D rhs)
        {
            return lhs.GetColumn(0) == rhs.GetColumn(0)
                   && lhs.GetColumn(1) == rhs.GetColumn(1)
                   && lhs.GetColumn(2) == rhs.GetColumn(2);
        }
        
        public static bool operator!=(Matrix2D lhs, Matrix2D rhs)
        {
            return !(lhs == rhs);
        }
        
        public static Vector2 operator*(Matrix2D lhs, Vector2 vector)
        {
            Vector2 res;
            res.x = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02;
            res.y = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12;
            return res;
        }
        
        public override string ToString()
        {
            return $"{m00:F5}\t{m01:F5}\t{m02:F5}\n{m10:F5}\t{m11:F5}\t{m12:F5}\n";
        }
        
        public string ToString(string format)
        {
            return $"{m00.ToString(format)}\t{m01.ToString(format)}\t{m02.ToString(format)}\n{m10.ToString(format)}\t{m11.ToString(format)}\t{m12.ToString(format)}\n";
        }
    }
}