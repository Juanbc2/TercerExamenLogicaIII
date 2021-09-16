using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Parcial3ILogicaIII
{
    public partial class Form2 : Form
    {

        int node;
        bool dirigido;
        static public LSL[] nodos;
        static int[] visitado;
        static int[,] caminos;
        public Form2(int node, bool dirigido)
        {
            InitializeComponent();
            this.node = node;
            this.dirigido = dirigido;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            numericUpDown1.Maximum = node;
            numericUpDown2.Maximum = node;
            nodos = new LSL[node];
            visitado = new int[node];
            caminos = new int[node, node];

        }

        private void fillZeros()
        {
            for (int i = 0; i < node; i++)
            {
                visitado[i] = 0;
            }
        }

        private void conectarVertices(int v1, int v2)
        {
            if (v1 == v2)
            {
                MessageBox.Show("No puede conectar un nodo a sí mismo.");
            }
            else
            {
                if (dirigido)
                {
                    if (nodos[v1 - 1] == null)
                    {
                        //Console.WriteLine("v1 nulo");
                        nodos[v1 - 1] = new LSL();
                        nodos[v1 - 1].setPrimerNodo(v2);
                        if (nodos[v2 - 1] == null)
                        {
                            //Console.WriteLine("v2 tambien nulo");
                            nodos[v2 - 1] = new LSL();
                            nodos[v2 - 1].setPrimerNodo(v1);
                        }
                        else
                        {
                            //Console.WriteLine("v1 nulo y v2 no");
                            nodos[v2 - 1].insertar(v1, nodos[v2 - 1].ultimoNodo());
                        }
                    }
                    else
                    {
                        //Console.WriteLine("v1 no nulo");
                        nodos[v1 - 1].insertar(v2, nodos[v1 - 1].ultimoNodo());
                        if (nodos[v2 - 1] == null)
                        {
                            //Console.WriteLine("v1 no nulo y v2 nulo");
                            nodos[v2 - 1] = new LSL();
                            nodos[v2 - 1].setPrimerNodo(v1);
                        }
                        else
                        {
                            //Console.WriteLine("v1 no nulo y v2 no nulo");
                            nodos[v2 - 1].insertar(v1, nodos[v2 - 1].ultimoNodo());
                        }
                    }
                }
                else
                {
                    if (nodos[v1 - 1] == null)
                    {
                        nodos[v1 - 1] = new LSL();
                        nodos[v1 - 1].setPrimerNodo(v2);
                    }
                    else
                    {
                        nodos[v1 - 1].insertar(v2, nodos[v1 - 1].ultimoNodo());
                    }
                }
            }

        }




        public void BFSlistasLigadasAdyacencia(int v)
        {

            int i = 0;
            int camino = v;

            int w;
            nodoSimple p;
            Stack<int> cola = new Stack<int>();
            visitado[v] = 1;
            cola.Push(v);
            while (cola.Count != 0)
            {
                v = (int)cola.Pop();
                caminos[camino, i] = v + 1; //camino desde V
                i++;
                if (nodos[v] != null)
                {
                    p = nodos[v].primerNodo(); //problema
                    while (p != null)
                    {
                        w = (int)p.retornaDato();
                        if (visitado[w - 1] == 0)
                        {
                            visitado[w - 1] = 1;
                            cola.Push(w - 1);
                        }
                        p = p.retornaLiga();
                    }
                }
            }

        }

        private bool rowIsInAarray(int[] row, ArrayList comps)
        {
            if (comps.Count == 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < comps.Count; i++) //comparar strings
                {
                    for (int j = 0; j < row.Length; j++)
                    {
                        int[] aux = (int[])comps[i];
                        if (row[j] != aux[j])
                        {
                            return false;
                        }
                        else if (areEqual(row, aux))
                        {
                            return true;
                        }
                    }

                }
            } Console.WriteLine("true");
            return true;
        }

        public static bool areEqual(int[] arr1, int[] arr2)
        {
            int n = arr1.Length;
            int m = arr2.Length;

            if (n != m)
                return false;

            Dictionary<int, int> map
                = new Dictionary<int, int>();
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                if (!map.ContainsKey(arr1[i]))
                    map.Add(arr1[i], 1);
                else
                {
                    count = map[arr1[i]];
                    count++;
                    map.Remove(arr1[i]);
                    map.Add(arr1[i], count);
                }
            }


            for (int i = 0; i < n; i++)
            {

                if (!map.ContainsKey(arr2[i]))
                    return false;


                if (map[arr2[i]] == 0)
                    return false;

                count = map[arr2[i]];
                --count;

                if (!map.ContainsKey(arr2[i]))
                    map.Add(arr2[i], count);
            }
            return true;
        }

        public void esConectado()
        {
            menorAMayor(caminos);
            ArrayList comps = new ArrayList();
            for (int i = 0; i < node; i++)
            {
                if (!rowIsInAarray(GetRow(caminos, i), comps))
                {
                    comps.Add(GetRow(caminos, i));
                }
            }
            
            String componentes = "";
            HashSet<string> hashSet = new HashSet<string>();
            
            if (comps.Count > 1)
            {
                foreach (int[] c in comps)
                {
                    string aux = "";
                    for (int i = 0; i < c.Length; i++)
                    {
                        aux += c[i].ToString();
                    }

                    hashSet.Add(aux);
                }

                foreach (string c in hashSet) {
                    string result = string.Join("", c);
                    componentes += " [" + result.TrimEnd(new char[] { '0' }) + "]\n";
                }
            }
            if (comps.Count == 1 && dirigido)
            {
                label4.Text = label4.Text + "dirigido y fuertemente conectado.";
                label6.Text = label6.Text + comps.Count;
            }
            else if (comps.Count == 1 && !dirigido)
            {
                label4.Text = label4.Text + "no dirigido y conectado.";
                label6.Text = label6.Text + comps.Count;
            }
            else if (dirigido)
            {
                label4.Text = label4.Text + "dirigido y no conectado.";
                label6.Text = label6.Text + hashSet.Count;
                label7.Text = "las componentes son:" + componentes;
            }
            else
            {
                label4.Text = label4.Text + "no dirigido y no conectado.";
                label6.Text = label6.Text + hashSet.Count;
                label7.Text = "las componentes son:" + componentes;
            }
            //(GetRow(caminos, i).SequenceEqual(GetRow(caminos, j)))

        }


        public void menorAMayor(int[,] al)
        {
            for (int i = 0; i < al.GetLength(0); i++)
            {
                int[] aux = GetRow(al, i);
                Array.Sort<int>(aux, new Comparison<int>(
          (i1, i2) => i2.CompareTo(i1)));
                setRow(aux, i);
            }
            //printmatrix(al);

        }

        public void setRow(int[] al, int rowNumber)
        {

            for (int j = 0; j < al.Length; j++)
            {
                caminos[rowNumber, j] = al[j];
            }
        }

        public int[] GetRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1)).Select(x => matrix[rowNumber, x]).ToArray();
        }

        private void printmatrix(int[,] arr)
        {
            int rowLength = arr.GetLength(0);
            int colLength = arr.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", arr[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }

        }


        private void printGrafo()
        {
            int i = 1;
            foreach (LSL list in nodos)
            {
                if (list != null)
                {
                    Console.WriteLine("nodo " + i);
                    list.mostrarLista();
                }
                else
                {
                    Console.WriteLine("nodo " + i + " no conectado");
                }
                i++;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = 0;
            while (i < node)
            {
                BFSlistasLigadasAdyacencia(i);
                fillZeros();
                i++;
            }
            esConectado();
            //printGrafo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conectarVertices((int)numericUpDown1.Value, (int)numericUpDown2.Value);
        }
    }



    //-----------------------------
    public class LSL
    {
        private nodoSimple primero, ultimo;
        public int n = 0; //testing

        public LSL()
        {

        }

        public nodoSimple primerNodo()
        {
            return this.primero;
        }
        public nodoSimple ultimoNodo()
        {
            return this.ultimo;

        }

        public void setPrimerNodo(int primero)
        {
            this.primero = new nodoSimple(primero);
            this.ultimo = this.primero;
        }

        public Boolean esVacia()
        {
            if (primero == null)
            {
                return true;
            }
            return false;
        }
        public Boolean finDeRecorrido(nodoSimple p)
        {
            if (p == null)
            {
                return true;
            }
            return false;
        }

        public nodoSimple anterior(nodoSimple x)
        {
            nodoSimple p = primerNodo();
            while (p != x)
            {
                p = p.retornaLiga();
            }
            return p;

        }

        public void mostrarLista()
        {
            nodoSimple p = primerNodo();
            while (!finDeRecorrido(p))
            {
                Console.Write(p.retornaDato() + ",");
                p = p.retornaLiga();
            }
            Console.WriteLine(".");
        }


        public nodoSimple buscarDondeInsertar(object d)
        {
            nodoSimple p = primerNodo();
            nodoSimple y = anterior(p);
            while (p != null)
            {
                if ((int)p.retornaDato() > (int)d)
                {
                    break;
                }
                y = p;
                p = p.retornaLiga();

            }
            return y;
        }
        public void insertar(object d, nodoSimple y)
        {
            nodoSimple x = new nodoSimple(d);
            conectar(x, y);

        }
        public void conectar(nodoSimple x, nodoSimple y)
        {
            n++; //testing
            if (y == null)
            {
                if (primero == null)
                {
                    primero = x;
                    ultimo = x;
                }
                else
                {
                    x.asignaLiga(primero);
                }
                return;
            }
            x.asignaLiga(y.retornaLiga());
            y.asignaLiga(x);
            if (y == ultimo) { ultimo = x; }
        }

        public nodoSimple buscarDato(object d, nodoSimple y)
        {
            nodoSimple x = primerNodo();
            while (!finDeRecorrido(x) && x.retornaDato() != d)
            {
                y.asignaDato(x);
                x = x.retornaLiga();
            }
            return x;
        }
        public void borrar(nodoSimple x, nodoSimple y)
        {
            if (x == null)
            {
                Console.WriteLine("no existe.");
                return;
            }
            desconectar(x, y);
        }
        public void desconectar(nodoSimple x, nodoSimple y)
        {
            if (y == null)
            {
                if (primero == ultimo)
                {
                    ultimo = null;
                    primero = x.retornaLiga();
                }
                else
                {
                    y.asignaLiga(x.retornaLiga());
                    if (x == ultimo)
                    {
                        ultimo = y;
                    }
                }
            }
        }



        public nodoSimple menorDato(nodoSimple y)
        {
            nodoSimple menor = primerNodo();
            nodoSimple q = menor;
            nodoSimple p = q.retornaLiga();
            while (!finDeRecorrido(p))
            {
                if ((int)p.retornaDato() < (int)menor.retornaDato())
                {
                    menor = p;
                    y.asignaDato(q);
                }
                q = p;
                p = p.retornaLiga();
            }
            return menor;
        }

        public void intercambiar(nodoSimple p, nodoSimple ap, nodoSimple q, nodoSimple aq)
        {
            if (p == q)
            {
                return;
            }
            desconectar(p, aq);
            if (ap == q)
            {
                conectar(p, aq);
            }
            else
            {
                if (aq == p)
                {
                    conectar(p, q);
                }
                else
                {
                    desconectar(q, aq);
                    conectar(p, aq);
                    conectar(q, ap);
                }
            }
        }

    }

    public class nodoSimple
    {
        private object dato;
        private nodoSimple liga;

        public nodoSimple(object d)
        {
            this.dato = d;
            liga = null;
        }

        public void asignaDato(object d)
        {
            this.dato = d;
        }

        public void asignaLiga(nodoSimple x)
        {
            this.liga = x;
        }

        public object retornaDato()
        {
            return this.dato;
        }

        public nodoSimple retornaLiga()
        {
            return this.liga;
        }
    }
}
