using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private GameObject goTerrain;
    private GameObject goPlayer;
    private GameObject goMapMarkerPlayer;
    private Terrain terrain;
    private RectTransform rtMap;
    private RectTransform rtMapMarkerPlayer;
    private float fPixMapPosHorzMapMarkerPlayer;
    private float fPixMapPosVertMapMarkerPlayer;
    private float fPixHalfSizeHorzMapMarkerPlayer;
    private float fPixHalfSizeVertMapMarkerPlayer;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goTerrain = GameObject.Find("Terrain");
        goPlayer = GameObject.Find("Player");
        goMapMarkerPlayer = transform.Find("RawImage : Map Marker Player").gameObject;
        terrain = goTerrain.GetComponent<Terrain>();
        rtMap = gameObject.GetComponent<RectTransform>();
        rtMapMarkerPlayer = goMapMarkerPlayer.GetComponent<RectTransform>();
        fPixHalfSizeHorzMapMarkerPlayer = rtMapMarkerPlayer.rect.width / 2f;
        fPixHalfSizeVertMapMarkerPlayer = rtMapMarkerPlayer.rect.height / 2f;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        fPixMapPosHorzMapMarkerPlayer = (goPlayer.transform.position.x / terrain.terrainData.size.x) * rtMap.rect.width;
        fPixMapPosVertMapMarkerPlayer = (goPlayer.transform.position.z / terrain.terrainData.size.z) * rtMap.rect.height;
        rtMapMarkerPlayer.SetInsetAndSizeFromParentEdge(
            RectTransform.Edge.Left,
            fPixMapPosHorzMapMarkerPlayer - fPixHalfSizeHorzMapMarkerPlayer,
            rtMapMarkerPlayer.rect.width
        );
        rtMapMarkerPlayer.SetInsetAndSizeFromParentEdge(
            RectTransform.Edge.Bottom,
            fPixMapPosVertMapMarkerPlayer - fPixHalfSizeVertMapMarkerPlayer,
            rtMapMarkerPlayer.rect.height
        );
        goMapMarkerPlayer.transform.SetPositionAndRotation(
            goMapMarkerPlayer.transform.position,
            new Quaternion(
                goMapMarkerPlayer.transform.rotation.x,
                goMapMarkerPlayer.transform.rotation.y,
                -goPlayer.transform.rotation.y,
                goMapMarkerPlayer.transform.rotation.w
            )
        );
    }

    // ------------------------------------------------------------------------------------------------

}
