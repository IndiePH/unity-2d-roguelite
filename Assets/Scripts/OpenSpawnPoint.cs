using System.Collections.Generic;

public static class OpenSpawnPoint
{
    public static int Index(IReadOnlyList<bool> available, int roll)
    {
        if (available == null || roll < 0)
            return -1;

        int seen = 0;
        for (int i = 0; i < available.Count; i++)
        {
            if (!available[i])
                continue;

            if (seen == roll)
                return i;

            seen++;
        }

        return -1;
    }
}
