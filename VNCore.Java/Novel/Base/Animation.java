package Novel.Base;

public abstract class Animation<AnimatedType> {
    AnimatedType from;
    AnimatedType to;
    /**
     * <h1>In milliseconds</h1>
     */
    Integer duration;
    Boolean autoReverse;

}