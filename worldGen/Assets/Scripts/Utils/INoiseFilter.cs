using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseFilter
{
    float Eval(Vector3 point);
}
