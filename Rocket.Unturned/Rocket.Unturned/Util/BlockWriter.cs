using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.Unturned.Util
{
    public class BlockWriter
    {
        private Block block;
        private int size;

        public BlockWriter(int prefix = 0)
        {
            block = new Block();
            block.reset(prefix);
        }

        public byte[] ToBytes()
        {
            return block.getBytes(out size);
        }

        public void Write(object objects)
        {
            block.write(objects);
        }

        public void Write(params object[] objects)
        {
           block.write(objects);
        }

        public void Dispose()
        {
            block = null;
        }
    }
}
