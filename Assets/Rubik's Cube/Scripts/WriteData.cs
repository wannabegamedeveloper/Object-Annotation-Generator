using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class WriteData : MonoBehaviour
{
    [SerializeField] private string location;
    [SerializeField] private float resX;
    [SerializeField] private float resY;
    [SerializeField] private int max;
    [SerializeField] private MeshRenderer cube;
    [SerializeField] private Transform cam;
    
    private Vector3 _topRight;
    private Vector3 _topLeft;
    private Vector3 _bottomRight;
    private Vector3 _bottomLeft;
    private StreamWriter _writer;

    private int _index;
    
    private float _maxX;
    private float _minX;
    private float _maxY;
    private float _minY;
    
    public void GenerateData()
    {
        if (_index >= max) return;
        cam.position = transform.position;
        GenerateBoundingBox(cube.bounds);
        Instantiate(cube, Camera.main.ScreenToWorldPoint(new Vector3(_minX, _maxY, 10f)), Quaternion.identity);
        WriteToFile();
    }
    
    private void GenerateBoundingBox(Bounds bounds) { 
        var c = bounds.center;
        var e = bounds.extents;
 
        Vector3[] worldCorners = new [] {
            new Vector3( c.x + e.x, c.y + e.y, c.z + e.z ),
            new Vector3( c.x + e.x, c.y + e.y, c.z - e.z ),
            new Vector3( c.x + e.x, c.y - e.y, c.z + e.z ),
            new Vector3( c.x + e.x, c.y - e.y, c.z - e.z ),
            new Vector3( c.x - e.x, c.y + e.y, c.z + e.z ),
            new Vector3( c.x - e.x, c.y + e.y, c.z - e.z ),
            new Vector3( c.x - e.x, c.y - e.y, c.z + e.z ),
            new Vector3( c.x - e.x, c.y - e.y, c.z - e.z )
        };
 
        IEnumerable<Vector3> screenCorners = worldCorners.Select(corner => Camera.main.WorldToScreenPoint(corner));
        Vector3[] enumerable = screenCorners as Vector3[] ?? screenCorners.ToArray();
        _maxX = enumerable.Max(corner => corner.x);
        _minX = enumerable.Min(corner => corner.x);
        _maxY = enumerable.Max(corner => corner.y);
        _minY = enumerable.Min(corner => corner.y);

        _maxX = Mathf.Clamp(_maxX, 0, resX);
        _minX = Mathf.Clamp(_minX, 0, resX);
        _maxY = Mathf.Clamp(_maxY, 0, resY);
        _minY = Mathf.Clamp(_minY, 0, resY);
    }

    private void WriteToFile()
    {
        _index++;
        OpenFile(location + _index + ".txt");
        WriteString(0 + " " + _minX + " " + (512f - _maxY) + " " + (_maxX - _minX) + " " + (_maxY - _minY));
    }
    
    private void OpenFile(string path)
    {
        _writer = new StreamWriter(path, true);
        _writer.AutoFlush = true;
    }

    private void WriteString(string text)
    {
        _writer.Write(text);
    }
}
