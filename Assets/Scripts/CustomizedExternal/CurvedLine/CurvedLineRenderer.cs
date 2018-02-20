using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof(LineRenderer) )]
public class CurvedLineRenderer : MonoBehaviour 
{
	//PUBLIC
	public float lineSegmentSize = 0.15f;
	public float lineWidth = 0.1f;
    public int disappearAmount = 2;
    public float disappearEvery = 0.01f;
    //[Header("Gizmos")]
    //public bool showGizmos = true;
    //public float gizmoSize = 0.1f;
    //public Color gizmoColor = new Color(1,0,0,0.5f);
    //PRIVATE
    LineRenderer line;
    private CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
	private Vector3[] linePositions = new Vector3[0];
	private Vector3[] linePositionsOld = new Vector3[0];

    private IEnumerator disappear;

    [HideInInspector]
    public bool disappearing = false;

    private void Awake()
    {
        disappear = Disappear();
        line = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    //public void Update()
    //{
    //    GetPoints();
    //    SetPointsToLine();
    //}

    public void Refresh()
    {
        GetPoints();
        SetPointsToLine();
    }

    public void Clear()
    {
        disappear = Disappear();
        StartCoroutine(disappear);
    }

    public void StopDisappear()
    {
        StopCoroutine(disappear);
        disappearing = false;
    }

    IEnumerator Disappear()
    {
        disappearing = true;
        while (line.positionCount > disappearAmount && disappearing)
        {
            Vector3[] positions = new Vector3[line.positionCount];
            line.GetPositions(positions);

            line.positionCount -= disappearAmount;
            for (int i = 0; i < line.positionCount; i++)
            {
                line.SetPosition(i, positions[i + disappearAmount]);
            }
            yield return new WaitForSeconds(disappearEvery);
        }
        line.positionCount = 0;
        disappearing = false;

    }

    void GetPoints()
	{
		//find curved points in children
		linePoints = this.GetComponentsInChildren<CurvedLinePoint>();

		//add positions
		linePositions = new Vector3[linePoints.Length];
		for( int i = 0; i < linePoints.Length; i++ )
		{
			linePositions[i] = linePoints[i].transform.position;
		}
	}

	void SetPointsToLine()
	{
		//create old positions if they dont match
		if( linePositionsOld.Length != linePositions.Length )
		{
			linePositionsOld = new Vector3[linePositions.Length];
		}

		//check if line points have moved
		bool moved = false;
		for( int i = 0; i < linePositions.Length; i++ )
		{
			//compare
			if( linePositions[i] != linePositionsOld[i] )
			{
				moved = true;
			}
		}

		//update if moved
		if( moved == true )
		{
            //get smoothed values
			Vector3[] smoothedPoints = LineSmoother.SmoothLine( linePositions, lineSegmentSize );

			//set line settings
			line.positionCount = smoothedPoints.Length;
			line.SetPositions( smoothedPoints );
            line.startWidth = lineWidth;
		}
	}

	//void OnDrawGizmosSelected()
	//{
	//	Update();
	//}

	//void OnDrawGizmos()
	//{
	//	if( linePoints.Length == 0 )
	//	{
	//		GetPoints();
	//	}

	//	//settings for gizmos
	//	foreach( CurvedLinePoint linePoint in linePoints )
	//	{
	//		linePoint.showGizmo = showGizmos;
	//		linePoint.gizmoSize = gizmoSize;
	//		linePoint.gizmoColor = gizmoColor;
	//	}
	//}
}
