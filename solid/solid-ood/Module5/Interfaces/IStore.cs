﻿using System.IO;

namespace SolidOod.Module5.Interfaces
{
    public interface IStore
    {

        Maybe<string> ReadAllText(int id);

        void Save(int id, string message);
    }
}