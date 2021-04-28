using System;
using System.Collections.Generic;
using System.Text;

namespace ArbolBinario
{
    public class AVL<T>: ICloneable
    {
        public AVL<T> Clone()
        {
            return new AVL<T>(this);
        }
        object ICloneable.Clone()
        {
            return Clone();
        }
        public AVL(AVL<T> a)
        {
            this.root = a.root.Clone();
            this.comparador = a.comparador;
            this.comparador2 = a.comparador2;
        }
        public AVL(Comparador<T> Funcomparador, Comparador2<T> Funcomparador2) //Esta es la funcion
        {
            this.comparador = Funcomparador;
            this.comparador2 = Funcomparador2; // el apuntador de la linea 12 que apunte a la funcion. 
        }
        internal Nodo<T> root;
        internal Comparador<T> comparador;
        internal Comparador2<T> comparador2;//una de las propiedades del arbol es el comparador

        public void Add(T dato)
        {
            if (root == null)
            {
                root = new Nodo<T>();
                root.Value = dato;
            }
            else
            {
                if (comparador.Invoke(root.Value, dato) > 0) //si es currentroot es mayor que el dato 
                {
                    if (root.Left == null) //si no hay nada guardado que guarde el nuevo. 
                    {
                        root.Left = new Nodo<T>();
                        root.Left.Value = dato;
                    }
                    else
                    {
                        Add(dato, root.Left); //recursividad, para que vaya al siguiente nivel y encuentre donde agregarlo. 
                    }
                }
                if (comparador.Invoke(root.Value, dato) < 0) //si es currentroot es menor que el dato 
                {
                    if (root.Right == null) //si no hay nada guardado que guarde el nuevo. 
                    {
                        root.Right = new Nodo<T>();
                        root.Right.Value = dato;
                    }
                    else
                    {
                        Add(dato, root.Right); //recursividad, para que vaya al siguiente nivel y encuentre donde agregarlo. 
                    }
                }
            }
            Balanceo();
        }
        public delegate int Comparador<T>(T a, T b);
        public delegate int Comparador2<T>(T a, T b);//El tipo del Apuntador del delegado
        private void Add(T dato, Nodo<T> CurrentRoot)
        {
            if (comparador.Invoke(CurrentRoot.Value, dato) > 0) //si es currentroot es mayor que el dato 
            {
                if (CurrentRoot.Left == null) //si no hay nada guardado que guarde el nuevo. 
                {
                    CurrentRoot.Left = new Nodo<T>();
                    CurrentRoot.Left.Value = dato;
                }
                else
                {
                    Add(dato, CurrentRoot.Left); //recursividad, para que vaya al siguiente nivel y encuentre donde agregarlo. 
                }
            }
            else if (comparador.Invoke(CurrentRoot.Value, dato) < 0) //si es currentroot es menor que el dato 
            {
                if (CurrentRoot.Right == null) //si no hay nada guardado que guarde el nuevo. 
                {
                    CurrentRoot.Right = new Nodo<T>();
                    CurrentRoot.Right.Value = dato;
                }
                else
                {
                    Add(dato, CurrentRoot.Right); //recursividad, para que vaya al siguiente nivel y encuentre donde agregarlo. 
                }
            }
            else
            {
                throw new Exception("Item repetido"); //ya que no puede haber otro con el mismo valor. 
            }
        }
        public void RemoveAt(T dato)
        {
            RemoveAt(dato, root, null);
            Balanceo();
        }
        private void RemoveAt(T dato, Nodo<T> currentRoot, Nodo<T> padre)
        {
            int p = 0;
            if (currentRoot is null) throw new Exception("No encontrado");
            var comparacion = comparador.Invoke(currentRoot.Value, dato);
            if (comparacion == 0)//ya encontro el que quiere borrar
            {
                if (currentRoot.EsHoja)
                {
                    if (padre is null)
                    {
                        root = null;
                        return;
                    }
                    if (padre.Left != null && padre.Right != null)
                    {
                        if (comparador(padre.Left.Value, dato) == 0)//si es el de la izquierda el que quiere borrar
                            padre.Left = null;
                        else padre.Right = null;
                    }
                    else
                    {
                        if (padre.Left != null)
                        {
                            padre.Left = null;
                        }
                        else
                        {
                            padre.Right = null;
                        }
                    }

                }
                else if (currentRoot.TieneDosHijos)
                {
                    Nodo<T> padreSucesor = BuscarPadredelSucesor(currentRoot); //currentRoot el que quiere eliminar
                    Nodo<T> sucesor = Sucesor(currentRoot);
                    if (padre == null)
                    {
                        padreSucesor.Right = sucesor.Left;
                        sucesor.Left = currentRoot.Left;
                        sucesor.Right = currentRoot.Right;
                        root = sucesor;
                    }
                    else
                    {
                        if (padreSucesor == currentRoot)
                        {
                            sucesor.Right = currentRoot.Right;
                            if (padre.Left != null && padre.Right != null)
                            {
                                if (comparador(padre.Left.Value, dato) == 0)
                                {
                                    padre.Left = sucesor;
                                }
                                else padre.Right = sucesor;
                            }
                            else
                            {
                                if (padre.Left != null)
                                {
                                    padre.Left = sucesor;
                                }
                                else
                                {
                                    padre.Right = sucesor;
                                }
                            }
                        }
                        else
                        {
                            padreSucesor.Right = sucesor.Left;
                            sucesor.Left = currentRoot.Left;
                            sucesor.Right = currentRoot.Right;

                            if (padre.Left != null && padre.Right != null)
                            {
                                if (comparador(padre.Left.Value, dato) == 0)
                                {
                                    padre.Left = sucesor;
                                }
                                else padre.Right = sucesor;
                            }
                            else
                            {
                                if (padre.Left != null)
                                {
                                    padre.Left = sucesor;
                                }
                                else
                                {
                                    padre.Right = sucesor;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (currentRoot.Left != null)
                    {
                        if (padre is null)
                        {
                            root = currentRoot.Left;
                        }
                        if (padre.Left != null && padre.Right != null)
                        {
                            if (comparador(padre.Left.Value, dato) == 0)//si es el de la izquierda el que quiere borrar
                                padre.Left = currentRoot.Left;
                            else padre.Right = currentRoot.Left;
                        }
                        else
                        {
                            if (padre.Left != null)
                            {
                                padre.Left = currentRoot.Left;
                            }
                            else
                            {
                                padre.Right = currentRoot.Left;
                            }
                        }
                    }
                    else
                    {
                        if (padre is null)
                        {
                            root = currentRoot.Right;
                        }
                        if (padre.Left != null && padre.Right != null)
                        {
                            if (comparador(padre.Left.Value, dato) == 0)//si es el de la izquierda el que quiere borrar
                                padre.Left = currentRoot.Right;
                            else padre.Right = currentRoot.Right;
                        }
                        else
                        {
                            if (padre.Left != null)
                            {
                                padre.Left = currentRoot.Right;
                            }
                            else
                            {
                                padre.Right = currentRoot.Right;
                            }
                        }
                    }
                }
                return;
            }
            if (comparacion > 0) RemoveAt(dato, currentRoot.Left, currentRoot); //que busque en el hijo izquierdo
            else if (comparacion < 0)
            {
                RemoveAt(dato, currentRoot.Right, currentRoot);
            }
        }
        private Nodo<T> Sucesor(Nodo<T> raiz)
        {
            Nodo<T> izquierdo = raiz.Left;
            if (izquierdo.EsHoja) return izquierdo; //ya encontro el ultimo que es el que va a reemplazar
            if (izquierdo.Right is null) return izquierdo;// ya encontro el mas derecho, mayor de los menores que va a reemplazar
            Nodo<T> actual = izquierdo.Right;
            while (actual.Right != null)
            {
                actual = actual.Right;
            }
            return actual;
        }
        private Nodo<T> BuscarPadredelSucesor(Nodo<T> raiz)
        {
            Nodo<T> izquierdo = raiz.Left;
            if (izquierdo.EsHoja) return raiz;
            if (izquierdo.Right is null) return raiz;
            Nodo<T> actual = izquierdo.Right;
            Nodo<T> padre = izquierdo;
            while (actual.Right != null)
            {
                padre = actual;
                actual = actual.Right;
            }
            return padre;
        }

        private Nodo<T> UbicarNodo(T buscador, Nodo<T> currentRoot)
        {
            if (currentRoot is null) return null;
            var comparacion = comparador.Invoke(currentRoot.Value, buscador);
            if (comparacion == 0) return currentRoot;
            if (comparacion > 0) return UbicarNodo(buscador, currentRoot.Left);
            return UbicarNodo(buscador, currentRoot.Right);
        }

        public List<T> ConvertirLista()
        {
            var list = new List<T>();
            ConvertirLista(list, root);
            return list;
        }
        private void ConvertirLista(List<T> lista, Nodo<T> CurrentRoot)
        {
            if (CurrentRoot != null)
            {
                ConvertirLista(lista, CurrentRoot.Left);
                lista.Add(CurrentRoot.Value);
                ConvertirLista(lista, CurrentRoot.Right);
            }
        }
        public T Find(T buscador)
        {
            return Find(buscador, root);
        }
        private T Find(T buscador, Nodo<T> currentRoot)
        {
            if (currentRoot is null) return default(T);
            var comparacion = comparador.Invoke(currentRoot.Value, buscador);
            if (comparacion == 0) return currentRoot.Value;
            if (comparacion > 0) return Find(buscador, currentRoot.Left);
            return Find(buscador, currentRoot.Right);
        }
        public T Find2(T buscador)
        {
            return Find2(buscador, root);
        }
        private T Find2(T buscador, Nodo<T> currentRoot)
        {
            if (currentRoot is null) return default(T);
            var comparacion = comparador2.Invoke(currentRoot.Value, buscador);
            if (comparacion == 0) return currentRoot.Value;
            if (comparacion > 0) return Find2(buscador, currentRoot.Left);
            return Find2(buscador, currentRoot.Right);
        }
        public int Altura(Nodo<T> nodo)
        {
            if (nodo.EsHoja) return 1;
            var vistaI = 1;
            var vistaD = 1;
            if (nodo.Left != null)
            {
                vistaI += Altura(nodo.Left);
            }
            if (nodo.Right != null)
            {
                vistaD += Altura(nodo.Right);
            }
            return Math.Max(vistaI, vistaD);
        }
        
       
        private void Balanceo()
        {
            var resultadoPivote = Pivote(root, root);
            if (resultadoPivote is null) return;
            var nodoPivote = resultadoPivote[0];
            var nodoPadrePivote = resultadoPivote[1];
            var alturaI = 0;
            var alturaD = 0;
            if (nodoPivote.Left != null)
            {
                alturaI = Altura(nodoPivote.Left);
            }
            if (nodoPivote.Right != null)
            {
                alturaD = Altura(nodoPivote.Right);
            }
            var FactorDeEquilibrio = 0;
            FactorDeEquilibrio = alturaI - alturaD;
            if (FactorDeEquilibrio > 1)
            {
                int derecha = 1;
                VerificarRotaciondobble(nodoPivote.Left, nodoPivote, derecha);
                RotacionDerecha(nodoPivote, nodoPadrePivote);
            }
            if (FactorDeEquilibrio < -1)
            {
                int izquirda = -1;
                VerificarRotaciondobble(nodoPivote.Right, nodoPivote, izquirda);
                RotacionIzquierda(nodoPivote, nodoPadrePivote);
            }

        }
        private Nodo<T>[] Pivote(Nodo<T> CurrentRoot, Nodo<T> padre)
        {
            if (CurrentRoot is null) //osea que no hay algo adentro
            {
                return null;
            }
            else
            {
                Nodo<T>[] resultadoI = Pivote(CurrentRoot.Left, CurrentRoot);
                if (resultadoI != null) return resultadoI;
                Nodo<T>[] resultadoD = Pivote(CurrentRoot.Right, CurrentRoot);
                if (resultadoD != null) return resultadoD;
                //recorrido post order
                var alturaI = 0;
                var alturaD = 0;
                if (CurrentRoot.Left != null)
                {
                    alturaI = Altura(CurrentRoot.Left);
                }
                if (CurrentRoot.Right != null)
                {
                    alturaD = Altura(CurrentRoot.Right);
                }
                var FactorDeEquilibrio = 0;
                FactorDeEquilibrio = alturaI - alturaD;

                if (FactorDeEquilibrio > 1 || FactorDeEquilibrio < -1)
                {
                    return new Nodo<T>[] { CurrentRoot, padre };
                }
                return null;
            }
        }
        private void VerificarRotaciondobble(Nodo<T> nodoPivote, Nodo<T> PadrePivote, int x)
        {
            var alturaI = 0;
            var alturaD = 0;
            if (nodoPivote.Left != null)
            {
                alturaI = Altura(nodoPivote.Left);
            }
            if (nodoPivote.Right != null)
            {
                alturaD = Altura(nodoPivote.Right);
            }
            var FactorDeEquilibrio = 0;
            FactorDeEquilibrio = alturaI - alturaD;
            if (x == 1)
            {
                if (FactorDeEquilibrio == -1)
                {
                    RotacionIzquierda(nodoPivote, PadrePivote);
                }
            }
            else
            {
                if (FactorDeEquilibrio == 1)
                {
                    RotacionDerecha(nodoPivote, PadrePivote);
                }
            }
        }

        private void RotacionIzquierda(Nodo<T> Pivote, Nodo<T> padrePivote)
        {
            if (Pivote == padrePivote.Left)
            {
                Nodo<T> i = new Nodo<T>();
                Nodo<T> f = new Nodo<T>();
                i = Pivote.Right.Left;
                f = Pivote;
                padrePivote.Left = Pivote.Right;
                Pivote = padrePivote.Left;
                Pivote.Left = f;
                Pivote.Left.Right = i;
            }
            else if (Pivote == padrePivote.Right)
            {
                Nodo<T> i = new Nodo<T>();
                Nodo<T> f = new Nodo<T>();
                i = Pivote.Right.Left;
                f = Pivote;
                padrePivote.Right = Pivote.Right;
                Pivote = padrePivote.Right;
                Pivote.Left = f;
                Pivote.Left.Right = i;
            }
            else
            {
                var i = Pivote.Right.Left;
                var f = Pivote;
                root = Pivote.Right;
                root.Left = f;
                root.Left.Right = i;
            }
        }
        private void RotacionDerecha(Nodo<T> Pivote, Nodo<T> padrePivote)
        {
            if (Pivote == padrePivote.Left)
            {
                Nodo<T> i = new Nodo<T>();
                Nodo<T> f = new Nodo<T>();
                i = Pivote.Left.Right;
                f = Pivote;
                padrePivote.Left = Pivote.Left;
                Pivote = padrePivote.Left;
                Pivote.Right = f;
                Pivote.Right.Left = i;
            }
            else if (Pivote == padrePivote.Right)
            {
                Nodo<T> i = new Nodo<T>();
                Nodo<T> f = new Nodo<T>();
                i = Pivote.Left.Right;
                f = Pivote;
                padrePivote.Right = Pivote.Left;
                Pivote = padrePivote.Right;
                Pivote.Right = f;
                Pivote.Right.Left = i;
            }
            else
            {
                var i = Pivote.Left.Right;
                var f = Pivote;
                root = Pivote.Left;
                root.Right = f;
                root.Right.Left = i;
            }
        }
    }
}
