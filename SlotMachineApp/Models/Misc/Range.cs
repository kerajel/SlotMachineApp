﻿namespace SlotMachineApp.Model;

public class Range<T>
{
    public Range(T from, T to)
    {
        From = from;
        To = to;
    }

    public T From { get; set; }
    public T To { get; set; }
}
