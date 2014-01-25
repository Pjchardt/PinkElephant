using System.Collections;

public class PCG
{
    protected byte[][] pcgrid; // Grid array
    protected int pcgrid_width; // Grid width
    protected int pcgrid_height; // Grid height

    public void updateParam(int g_width, int g_height)
    {
        pcgrid_width = g_width; // Get grid length
        pcgrid_height = g_height; // Get grid width
    }

    public void generatePCG(byte[][] g)
    {
        pcgrid = g; // Copy grid
    }

    protected bool bounded(int x, int y)
    {
        // Check if cell is inside grid
        if (x < 0 || x >= pcgrid_width || y < 0 || y >= pcgrid_height) return false;
        return true;
    }

    protected bool blocked(int x, int y, int type)
    {
        // Check if cell is occupied
        if (bounded(x, y) && pcgrid[x][y] == type) return true;
        return false;
    }
}