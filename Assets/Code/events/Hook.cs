public abstract class Hook<TImplementor> : IHook where TImplementor : IHook {

    /// <summary>
    /// Call this this hook on the EventDispatcher.
    /// </summary>
    public Hook<TImplementor> Call() {
        EventDispatcher.Instance.Call<TImplementor>((IHook)this);
        return this;
    }
}

