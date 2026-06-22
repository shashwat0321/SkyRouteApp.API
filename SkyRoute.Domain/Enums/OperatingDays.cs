namespace SkyRoute.Domain.Enums
{
    [System.Flags]
    public enum OperatingDays
    {
        None = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64
    }
}

/* we are using here powers of 2 - so that in binary -Each value has exactly one bit set to 1:
        1 = 0000001(bit 0)
        2 = 0000010(bit 1)
        4 = 0000100(bit 2)
        8 = 0001000(bit 3)
        16 = 0010000(bit 4)
        32 = 0100000(bit 5)
        64 = 1000000(bit 6)
    Now every OR combination can produce a unique Number:-
        Mon + Wed = 1 | 4 = 0000101 = 5
        Mon + Fri = 1 | 16 = 0010001 = 17
        Wed + Fri = 4 | 16 = 0010100 = 20
        Mon + Wed + Fri = 1 | 4 | 16 = 0010101 = 21

    Also you can use AND operator to check if a particular is present in the combination or not
THINKK!!!
*/