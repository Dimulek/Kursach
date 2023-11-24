using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightningTower : ShootingTower
{
    [SerializeField]
    private float stunTime;

    [SerializeField]
    private float stunDuration;

    [SerializeField]
    private GameObject lineRender;

    private List<GameObject> lineList = new List<GameObject>();

    private void linesDestroy()
    {
        for(int i = lineList.Count - 1; i>=0; --i)
            Destroy(lineList[i]);
    }
    private void FixedUpdate()
    {
        if (getNearEnemy())
        {
            if (isCanFire)
            {
                StartCoroutine(shootEffect());
                StartCoroutine(shootEnemy());
            }
        }
    }

    private IEnumerator shootEnemy()
    {
        isCanFire = false;
        for (int i = nearEnemy.Count - 1; i >= 0; --i)
            nearEnemy.ElementAt(i).GetComponent<Enemy>().StunEnemy(damage, stunTime, stunDuration);
        yield return new WaitForSeconds(fireTransaction / 1f);
        isCanFire = true;
    }
    private IEnumerator shootEffect()
    {
        drawLine(setZUpper(transform.position, -0.2f));
        yield return new WaitForSeconds(0.1f);
        linesDestroy();
        lineList.Clear();
    }


    private Vector3 setZUpper(in Vector3 vec, in float zUpper)
    {
        Vector3 newVec = vec;
        newVec.z += zUpper;
        return newVec;
    }

    private void drawLine(in Vector3 vecThis)
    {
        foreach (GameObject obj in nearEnemy.OrderBy(p => Vector3.Distance(transform.position, p.transform.position)).Take(3))
        {
            GameObject line = Instantiate(lineRender, transform.position, transform.rotation, transform);
            lineList.Add(line);
            LineRenderer lineComponent = line.GetComponent<LineRenderer>();
            lineComponent.SetPosition(0, vecThis);
            lineComponent.SetPosition(1, setZUpper(obj.transform.position, -0.2f));
        }
    }
}
