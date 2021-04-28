using System;
using System.Collections.Generic;
using System.Text;

namespace ArbolBinario
{
    public class Nodo<T> : ICloneable
    {
        public T Value;

        public Nodo<T> Left;
        public Nodo<T> Right;
        public Nodo<T> Clone()
        {
            return new Nodo<T>(this);
        }
        object ICloneable.Clone()
        {
            return Clone();
        }
        public Nodo(Nodo<T> n)
        {
            this.Left = n.Left != null ? n.Left.Clone() : n.Left;
            this.Right = n.Right != null ? n.Right.Clone() : n.Right;
            this.Value = n.Value;
            //this.TieneDosHijos = n.TieneDosHijos;
            //this.EsHoja = n.EsHoja;
        }

        public Nodo()
        {
            this.Left = null;
            this.Right = null;
        }

        public bool EsHoja
        {
            get
            {
                return Left is null && Right is null;
            }
        }
        public bool TieneDosHijos
        {
            get
            {
                return Left != null && Right != null;
            }
        }

    }
}
