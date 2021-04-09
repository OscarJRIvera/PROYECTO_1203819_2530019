using System;
using System.Collections.Generic;
using System.Text;

namespace ArbolDePrioridad
{
    class ArbolDePrioridad<T>
    {
        public delegate int Comparador<T>(T a, T b);
        public ArbolDePrioridad(Comparador<T> Funcomparador) //Esta es la funcion
        {
            this.comparador = Funcomparador; // el apuntador de la linea 12 que apunte a la funcion. 
        }
        internal Nodo<T> root;
        bool Ordenado;
        internal Comparador<T> comparador;

        bool Comprobacion;
        int CantidadNodos;
        public bool isempty()
        {
            if (root == null)
            {
                return true;
            }
            return false;
        }
        public T Peek()
        {
            return root.Value;
        }
        public void add(T dato)
        {
            Comprobacion = false;
            CantidadNodos++;
            add2(dato, root);
        }
        public void add2(T dato, Nodo<T> CurrentRoot)
        {
            bool Left = CantidadNodos / 2 == CurrentRoot.Pos;
            bool Right = CantidadNodos - 1 / 2 == CurrentRoot.Pos;
            if (CurrentRoot == null)
            {
                CurrentRoot.Pos = CantidadNodos;
                Comprobacion = true;
                CurrentRoot.Value = dato;
            }
            else if (CurrentRoot.Left.Value != null && CurrentRoot.Right.Value != null) {
                add2(dato, CurrentRoot.Left);
                if (Comprobacion == false)
                {
                    add2(dato, CurrentRoot.Right);
                }
            }
            else if (CurrentRoot.Left.Value == null)
            {
                if (Left == true)
                {
                    add2(dato, CurrentRoot.Left);
                }
            }
            else if (CurrentRoot.Right.Value == null)
            {
                if (Right == true)
                {
                    add2(dato, CurrentRoot.Right);
                }
            }
            ComprobarOrden(CurrentRoot);
        }
        public void ComprobarOrden(Nodo<T> CurrentRoot)
        {
            if (CurrentRoot.Left.Value != null || CurrentRoot.Right.Value != null)
            {
                Nodo<T> Temp = new Nodo<T>();
                Temp = CurrentRoot;
                if (CurrentRoot.Left != null)
                {
                    if (comparador.Invoke(CurrentRoot.Left.Value, CurrentRoot.Value) == 1)
                    {
                        CurrentRoot.Value = CurrentRoot.Left.Value;
                        CurrentRoot.Left.Value = Temp.Value;
                    }
                    else
                    {
                        Ordenado = false;
                    }
                }
                else if (CurrentRoot.Right != null)
                {
                    if (comparador.Invoke(CurrentRoot.Right.Value, CurrentRoot.Value) == 1)
                    {
                        CurrentRoot.Value = CurrentRoot.Right.Value;
                        CurrentRoot.Right.Value = Temp.Value;
                    }
                    else
                    {
                        Ordenado = false;
                    }
                }
            }
        }
        public T Remove()
        {
            Nodo<T> Temp;
            Temp = root;
            if (root.Left == null)
            {
                root = null;
            }
            else
            {
                Remove2(Temp);
            }
            OrdenarEliminacion(root);
            CantidadNodos--;
            return Temp.Value;

        }
        public void Remove2(Nodo<T> CurrentRoot)
        {
            if (CurrentRoot.EsHoja)
            {
                root.Value = CurrentRoot.Value;
                CurrentRoot = null;
            }
            else
            {
                int Cant = CantidadNodos;
                double nivel1 = Math.Truncate(Math.Log2(CurrentRoot.Pos + 1));
                double nivel2 = Math.Truncate(Math.Log2(CantidadNodos + 1));
                for (int i = Convert.ToInt32(nivel1); i < nivel2; nivel2++)
                {
                    Cant = Cant / 2;
                }
                if (Cant % 1 == 0)
                {
                    Remove2(CurrentRoot.Left);
                }
                else
                {
                    Remove2(CurrentRoot.Right);
                }
            }
        }
        public void OrdenarEliminacion(Nodo<T> CurrentRoot)
        {
            if (CurrentRoot.TieneDosHijos)
            {
                int i = comparador.Invoke(CurrentRoot.Left.Value, CurrentRoot.Right.Value);
                if (i == 1)
                {
                    Nodo<T> temp = CurrentRoot;
                    CurrentRoot.Value = CurrentRoot.Left.Value;
                    CurrentRoot.Left.Value = temp.Value;
                    OrdenarEliminacion(CurrentRoot.Left);
                }
                else
                {
                    Nodo<T> temp = CurrentRoot;
                    CurrentRoot.Value = CurrentRoot.Right.Value;
                    CurrentRoot.Right.Value = temp.Value;
                    OrdenarEliminacion(CurrentRoot.Right);
                }
            }
            else if (!CurrentRoot.EsHoja)
            {
                Nodo<T> temp = CurrentRoot;
                CurrentRoot.Value = CurrentRoot.Left.Value;
                CurrentRoot.Left.Value = temp.Value;
            }

        }
    }
}
