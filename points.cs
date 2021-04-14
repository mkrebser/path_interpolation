using UnityEngine;

public static class Points
{
    /// <summary>
    /// Interpolate along an array of 3D points.
    /// </summary>
    /// <param name="points">Array of points. </param>
    /// <param name="index">Index of point that will be interpolated around. </param>
    /// <param name="t"> Interoplation parameter 't' for the desired point and the next point in the array. </param>
    /// <param name="position"> output interpolated position. </param>
    /// <param name="direction"> output interpolated direction. </param>
    /// <param name="loop"> Does this array of points create a loop? </param>
    /// <param name="average"> Do point neighborhood averaging? </param>
    /// <param name="avg_weight"> Weighting factor used for point averaging. </param>
    /// <param name="smoothing"> Enable catmull rom spline for the array of points? </param>
    /// <param name="start_index"> Start index in the input array of points to use. </param>
    /// <param name="count"> Count of points to use. A value of '-1' will just default to the input array length. </param>
    public static void InterpolatePoints(in Vector3[] points, in int index, in float t, out Vector3 position, out Vector3 direction, in bool loop = false, in bool average = true, in float avg_weight = 0.5f, in bool smoothing = true, in int start_index = 0, int count = -1)
    {
        // A good reference guide for catmull rom spline https://www.mvps.org/directx/articles/catmull/

		count = count < 0 ? points.Length : count;

		if (count < 2)
			throw new System.Exception("Error, must use atleast 2 points!");

		int index_p0 = index - 1 < 0 ? (loop ? count - 1 : index) : index - 1; // indices of 4 points needed for catmull rom
		int index_p1 = index;
		int index_p2 = index + 1 >= count ? (loop ? 0 : index) : index + 1;
		int index_p3 = index_p2 + 1 >= count ? (loop ? 0 : index_p2) : index_p2 + 1;

		int index_p0m1 = index_p0 - 1 < 0 ? (loop ? count - 1 : index_p0) : index_p0 - 1; // extra indices used for the weighted sum
		int index_p3p1 = index_p3 + 1 >= count ? (loop ? 0 : index_p3) : index_p3 + 1;

		float avg_weight2 = (1 - avg_weight) * 0.5f; // 'Averaging', if enabled, does a weighted sum of the desired point and its left & right neighbors.
		bool move_pt = average && (loop || (index != 0));
		bool move_next_pt = average && (loop || index_p2 < count - 1);
		Vector3 p0 = average                 ? (points[start_index + index_p0m1] + points[start_index + index_p1]) * avg_weight2 + points[start_index + index_p0] * avg_weight : points[start_index + index_p0];
		Vector3 p1 = average && move_pt      ? (points[start_index + index_p0]   + points[start_index + index_p2]) * avg_weight2 + points[start_index + index_p1] * avg_weight : points[start_index + index_p1];
		Vector3 p2 = average && move_next_pt ? (points[start_index + index_p1]   + points[start_index + index_p3]) * avg_weight2 + points[start_index + index_p2] * avg_weight : points[start_index + index_p2];
		Vector3 p3 = average                 ? (points[start_index + index_p2] + points[start_index + index_p3p1]) * avg_weight2 + points[start_index + index_p3] * avg_weight : points[start_index + index_p3];

		Vector3 a0 = 2 * p1;
		Vector3 a1 = p2 - p0;
		Vector3 a2 = 2 * p0 - 5 * p1 + 4 * p2 - p3;
		Vector3 a3 = 3 * p1 - 3 * p2 + p3 - p0;

		float t_2 = t * t;
		float t_3 = t_2 * t;

		position = smoothing ? 0.5f * (a0 + a1 * t + a2 * t_2 + a3 * t_3) : p1 * (1-t) + p2 * t; // if smoothing-> do catmull rom, otherwise do simple interpolation
		direction = (smoothing ? 0.5f * (a1 + 2 * a2 * t + 3 * a3 * t_2) : p2 - p1).normalized; // if smoothing-> do derivative of catmull rom, otherwise just subtract
    }
}
