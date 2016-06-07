package Novel;

public class Position {
    public Byte x;
    public Byte y;
    public Byte width;
    public Byte height;

    @Override
    public String toString() {
        return String.format("%1d %2d %3d %4d", x, y, width, height);
    }

    public static Position parse(String data) {
        String[] dataArray = data.split(" ");
        Position result = new Position();
        result.x = Byte.parseByte(dataArray[0]);
        result.y = Byte.parseByte(dataArray[1]);
        result.width = Byte.parseByte(dataArray[2]);
        result.height = Byte.parseByte(dataArray[3]);
        return result;
    }

}