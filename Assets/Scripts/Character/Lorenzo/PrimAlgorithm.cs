using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimAlgorithm : MonoBehaviour
{
    public GameObject lightningEffect;

    public LayerMask enemyLayer;
    private float electricDamage = 125f;
    private int V = 5;
    private int[] parent;
    private List<GameObject> vertex;
    private Collider[] hitColliders;
    private Collider[] prevHitColliders;

    private int MinKey(float[] key, bool[] mstSet)
    {
        float min = float.MaxValue;
        int min_index = -1;

        for (int v = 0; v < V; v++)
        {
            if (mstSet[v] == false && key[v] < min)
            {
                min = key[v];
                min_index = v;
            }
        }
        return min_index;
    }

    private void Prim(List<List<float>> graph)
    {
        parent = new int[V];
        float[] key = new float[V];
        bool[] mstSet = new bool[V];

        for (int i = 0; i < V; i++)
        {
            key[i] = float.MaxValue;
            mstSet[i] = false;
        }

        key[0] = 0;
        parent[0] = -1;

        if (V < 2)
        {
            parent[0] = 0;
        }

        for (int count = 0; count < V - 1; count++)
        {
            int u = MinKey(key, mstSet);
            //Debug.Log(u + " -- " + mstSet[u]);
            
            mstSet[u] = true;

            for (int v = 0; v < V; v++)
            {
                if ((graph[u])[v] != 0 && mstSet[v] == false
                    && (graph[u])[v] < key[v])
                {
                    Debug.Log("v:" + v);
                    parent[v] = u;
                    key[v] = (graph[u])[v];
                }
            }
        }

        //Debug.Log(hitColliders.Length + " - " + graph.Count);
        //Debug.Log("Edge \tWeight");
        //for (int i = 1; i < V; i++)
        //    Debug.Log(parent[i] + " - " + i + "\t" + (graph[i])[parent[i]]);
    }

    private void Init()
    {
        vertex.Clear();

        hitColliders = Physics.OverlapSphere(transform.position, 10f, enemyLayer);

        if (hitColliders.Length <= 0)
        {

            return;
        }

        foreach (Collider c in hitColliders)
        {
            Debug.Log(c.gameObject.name);
            vertex.Add(c.gameObject);
        }
        vertex.Add(gameObject);

        V = vertex.Count;

        List<List<float>> adjacentList = new List<List<float>>();
        foreach (GameObject x in vertex)
        {
            List<float> row = new List<float>();
            foreach (GameObject y in vertex)
            {
                float weight;
                //Debug.Log(x.name.Equals(y.name));
                if (x.name.Equals(y.name))
                {
                    weight = 0;
                }
                else
                {
                    weight = Vector3.Distance(y.transform.position, x.transform.position);
                }
                row.Add(weight);
            }
            adjacentList.Add(row);
        }
        Prim(adjacentList);
        //StartEffect();
    }

    void StartEffect()
    {
        List<List<string>> vertexConnections = new List<List<string>>(V);
        for (int i = 0; i < V; i++)
        {
            vertexConnections.Add(new List<string>());
        }

        for (int i = 0; i < V; i++)
        {
            string v1 = hitColliders[i].name;
            Debug.Log(V);
            Debug.Log(hitColliders.Length + " - " + i + " - " + parent[i+1]);
            string v2 = hitColliders[parent[i]].name;
            vertexConnections[i].Add(v2);

            
            GameObject l = Instantiate(lightningEffect, hitColliders[parent[i]].transform.position, Quaternion.identity);
            Debug.Log(l.name);

            if (V > 1)
            {
                vertexConnections[parent[i]].Add(v1);
            }
        }
        for (int i = 0; i < V; i++)
        {
            Target t = hitColliders[i].GetComponent<Target>();
            //t.TakeDamage(electricDamage * vertexConnections[i].Count);
        }
        //skillPoint -= 75;
    }

    private void Start()
    {
        vertex = new List<GameObject>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Init();
        }
    }
}
