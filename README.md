# path_interpolation
Function to interpolate between points in an array of points. (Eg, smoothly interpolate along a 3D path of points)

![Alt Text](https://github.com/mkrebser/path_interpolation/blob/master/points.gif)

This function was made primarily for Unity3D, but should be easy enough to translate to a new environment if needed.
A mixture of point averaging and catmull rom are used for interpolation. To do interpolation, just create an array of 3D points and specify an index and interpolation constant 't'.
The function will return some smoothed/interpolated value between the desired point and the next one.
