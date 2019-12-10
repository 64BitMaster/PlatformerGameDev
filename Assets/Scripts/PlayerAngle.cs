

public class PlayerAngle
{
    public static bool angle;
    public static bool isUp;
    // Start is called before the first frame update

    static public void setAngle(bool x)
    {
        angle = x;
    }
    static public bool getAngle() { return angle; }
    static public void setIsUp(bool x)
    {
        isUp = x;
    }
    static public bool getIsUp() { return isUp; }

}
