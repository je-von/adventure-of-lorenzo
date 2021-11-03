using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimAlgorithm : MonoBehaviour
{
    public TMPro.TextMeshProUGUI warningText;

    public GameObject lightningEffect;

    public LayerMask enemyLayer;
    private int electricDamage = 125;
    private int V = 5;
    private int[] parent;
    private List<GameObject> vertex;
    private Collider[] hitColliders;
    private Collider[] prevHitColliders;

    float radius = 10f;
    public LineRenderer line;
    

    bool hasCheckedRange = false;

    private int MinKey(float[] key, bool[] mstSet)
    {
        float min = float.MaxValue;
        int min_index = -1;

        for (int v = 0; v < V; v++)
        {
            if (mstSet[v] == false && key[v] < min)
            {
                Debug.LogFormat("key["+ v + "] = " + key[v]);
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
        Debug.Log("Edge \tWeight");
        for (int i = 1; i < V; i++)
            Debug.Log(parent[i] + " - " + i + "\t" + (graph[i])[parent[i]]);
    }

    private Collider[] CheckEnemyInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        if (colliders.Length <= 0)
        {
            Debug.Log("no enemy");
            return null;
        }

        return colliders;
    }

    private void StartAttack()
    {
        vertex.Clear();

        //hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        //if (hitColliders.Length <= 0)
        //{
        //    Debug.Log("no enemy");
        //    return;
        //}

        foreach (Collider c in hitColliders)
        {
            Debug.Log(c.gameObject.name);
            vertex.Add(c.gameObject);
        }
        vertex.Add(gameObject);
        Debug.Log(gameObject.name);

        V = vertex.Count;
        Debug.Log("Vertex: " + V);
        List<List<float>> adjacentList = new List<List<float>>();

        Debug.Log("=============");
        foreach (GameObject x in vertex)
        {
            List<float> row = new List<float>();
                string str = "";
            foreach (GameObject y in vertex)
            {
                float weight;
                //Debug.Log(x.name.Equals(y.name));
                if (x.Equals(y))
                {
                    weight = 0;
                }
                else
                {
                    weight = Vector3.Distance(y.transform.position, x.transform.position);
                }
                str += " " + weight + " ";
                row.Add(weight);
            }
            Debug.Log(str);
            adjacentList.Add(row);
        }
        Debug.Log("=============");


        Prim(adjacentList);
        StartEffect();
    }

    void StartEffect()
    {
        List<int> connectionCount = new List<int>(V);
        
        for (int i = 0; i < V; i++)
        {
            connectionCount.Add(0);
        }

        Debug.Log("List of Vertex");
        foreach(GameObject g in vertex)
        {
            Debug.Log(g.name);
        }

        for (int i = 1; i < V; i++)
        {
            Debug.Log("Index: " + i);
            var v1 = vertex[i];
            Debug.Log("Collider1: "  + v1.name);
            var v2 = vertex[parent[i]];
            Debug.Log("Collider2: " + v2.name);

            connectionCount[i]++;
            
            GameObject l = Instantiate(lightningEffect, vertex[parent[i]].transform.position, Quaternion.identity);

            DigitalRuby.LightningBolt.LightningBoltScript lbs = l.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>();
            lbs.StartObject = vertex[parent[i]].gameObject;
            lbs.EndObject = vertex[i].gameObject;

            //lbs.gameObject.transform.position = new Vector3(lbs.gameObject.transform.position.x, lbs.gameObject.transform.position.y + 10, lbs.gameObject.transform.position.z);
            Destroy(l, 6f);


            Debug.Log(l.name);

            if (V > 1)
            {
                connectionCount[parent[i]]++;


            }
        }
        for (int i = 0; i < V; i++)
        {

            int points = electricDamage * connectionCount[i];
            if(vertex[i].GetComponent<EnemyController>() != null) {
                var enemy = vertex[i].GetComponent<EnemyController>();
                StartCoroutine(DecreaseEnemyHealth(enemy, points));
            }
        }
        //Lorenzo.GetInstance().skillPoints -= 75;
        //Lorenzo.GetInstance().DecreaseSkillPoint(75);
    }

    IEnumerator DecreaseEnemyHealth(EnemyController e, float points)
    {
        float interval = points / 6f;
        while (points > 0)
        {
            yield return null;
            e.enemy.healthPoints -= Time.deltaTime * interval;
            points -= Time.deltaTime * interval;
        }
    }

    //IEnumerator DecreaseEnemyHealth(WarriorController w, float points)
    //{
    //    float interval = points / 6f;
    //    while (points > 0)
    //    {
    //        yield return null;
    //        w.enemy.healthPoints -= Time.deltaTime * interval;
    //        points -= Time.deltaTime * interval;
    //    }
    //}

    void StarMarking()
    {
        int segments = 40;
        line.positionCount = segments + 1;
        //radiusLine.SetVertexCount(segments + 1);
        line.useWorldSpace = false;

        float x, z, deg = 20f;

        for (int i = 0; i <= segments; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * deg) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * deg) * radius;

            line.SetPosition(i, new Vector3(x, 0, z));
            Debug.Log("line:" + line.GetPosition(i));
            deg += (360f / segments);
        }

        line.gameObject.SetActive(false);

        StartCoroutine(ActivateRadiusLine());
        StartCoroutine(ActivateBombSprite());
    }

    IEnumerator ActivateRadiusLine()
    {
        line.gameObject.SetActive(true);
        hasCheckedRange = true;

        yield return new WaitForSeconds(5f);

        line.gameObject.SetActive(false);
        hasCheckedRange = false;
    }

    IEnumerator ActivateBombSprite()
    {
        float startTime = Time.time;
        float timeElapsed = 0;
        while (timeElapsed <= 5)
        {
            timeElapsed = Time.time - startTime;
            hitColliders = null;
            hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);
            V = hitColliders.Length;
            if (prevHitColliders != null)
            {
                foreach (var prev in prevHitColliders)
                {
                    bool flag = false;
                    foreach (var h in hitColliders)
                    {
                        if (prev.Equals(h))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        prev.GetComponent<EnemyController>().SetEnemyInAttackRange(false);
                    }
                }
            }
            foreach (var h in hitColliders)
            {
                h.GetComponent<EnemyController>().SetEnemyInAttackRange(true);
            }

            prevHitColliders = hitColliders;
            yield return new WaitForSeconds(0);
        }
        foreach (var prev in prevHitColliders)
        {
            prev.GetComponent<EnemyController>().SetEnemyInAttackRange(false);
        }
    }

    private void Start()
    {
        vertex = new List<GameObject>();
    }
    private void Update()
    {
        if (DialogueController.isShowing)
            return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            hitColliders = CheckEnemyInRange();
            if (hitColliders == null)
            {
                StartCoroutine(ShowWarningText());
                return;
            }

            if (hasCheckedRange)
            {
                Debug.Log("z dua kali");

                if (Lorenzo.GetInstance().DecreaseSkillPoint(75))
                {
                    StartAttack();
                }

            }
            else
            {
                Debug.Log("z sekali");
                StarMarking();
            }

        }
    }

    IEnumerator ShowWarningText()
    {
        warningText.text = "NO ENEMY IN RADIUS";
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        warningText.gameObject.SetActive(false);

    }
}
