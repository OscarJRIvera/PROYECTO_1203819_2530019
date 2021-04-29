using System;
using DoubleLinkedListLibrary1;
using System.Text;
using System.IO;
using Newtonsoft;

namespace TablaHash
{
    public class TablaHash<K, V>
    {
        int largoTabla;
        int funcionHash(K llave)
        {
            Int64 temp2 = Convert.ToInt64(llave);
            string temp = Convert.ToString(llave);
            int size = temp.Length;
            Int64[] bytes = new Int64[size];
            for (int index = size - 1; index >= 0; index--)
            {
                bytes[index] = temp2 % 10;
                temp2 = temp2 / 10;
            }
            long contador = 1;
            long contador2 = 2;
            long x = 0;

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] % 2 == 0)
                {
                    contador += Convert.ToInt64(bytes[i]);
                }
                else
                    contador2 += Convert.ToInt64(bytes[i]);
            }
            x = (contador * bytes.Length) * contador2;
            return Convert.ToInt32(x) % largoTabla;
        }
        DoubleLinkedList<LlaveValor<V>> Diccionario;
        public TablaHash(int count, Comparador<V> Funcomparador) 
        {

            this.comparador = Funcomparador;
            Diccionario = new DoubleLinkedList<LlaveValor<V>>();
            largoTabla = count;
            for (int i = 0; i < largoTabla; i++)
            {
                Diccionario.Add(new LlaveValor<V>(i));
            }
        }
        internal Comparador<V> comparador;
        public delegate int Comparador<V>(V a, Int64 b);

        public void Add(K llave, V valor)
        {
            int hash = funcionHash(llave);
            var llaveValor = Diccionario.Find(p => p.Llave.Equals(hash));
            llaveValor.Valor.Add(valor);
        }
        public void Remove(K llave)
        {
            var hash = funcionHash(llave);
            var llaveValor = Diccionario.Find(p => p.Llave.Equals(hash));
            int posicion = llaveValor.Valor.Find2(m => comparador(m,Convert.ToInt64(llave)) == 0);
            llaveValor.Valor.RemoveAt(posicion);

        }
        public V Find(K llave)
        {
            var hash = funcionHash(llave);
            var llaveValor = Diccionario.Find(p => p.Llave.Equals(hash));
            var I = llaveValor.Valor.Find(m => comparador(m, Convert.ToInt64(llave)) == 0);
            return I;
        }
        public V Remove2(int posi, int posi2)
        {
            var llaveValor = Diccionario.Find(p => p.Llave.Equals(posi));
            var I = llaveValor.Valor.RemoveAt2(posi2);
            return I;
        }
        public int BuscarCanti(int posi)
        {
            var llaveValor = Diccionario.Find(p => p.Llave.Equals(posi));
            var I = llaveValor.Valor.Count2();
            return I;
        }
        public int getsize()
        {
            return largoTabla;
        }
    }
}
