namespace ArcadeServer
{
    public abstract class Command
    {
        private readonly ushort id;

        protected Command(ushort id)
        {
            this.id = id;
        }

        protected abstract void Encode(BinaryWriter writer);

        protected abstract void Decode(BinaryReader reader);

        protected abstract void Process(Client client);

        public void EncodeCommand(BinaryWriter writer)
        {
            writer.Write(this.id);

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryWriter writer2 = new BinaryWriter(stream);

                this.Encode(writer2);

                writer.Write((int)stream.Length);
                writer.Write(stream.ToArray());
            }
        }

        public static void DecodeCommand(Client client, BinaryReader reader)
        {
            try
            {
                ushort id = reader.ReadUInt16();
                Command command = Commands.GetCommand(id);

                int length = reader.ReadInt32();
                byte[] data = reader.ReadBytes(length);

                if (command == null)
                {
                    Console.WriteLine($"Unknown command {id}.");
                    return;
                }

                using (MemoryStream stream = new MemoryStream(data))
                {
                    using (BinaryReader reader2 = new BinaryReader(stream))
                    {
                        command.Decode(reader2);
                    }
                }

                Console.WriteLine($"Received client command {command.GetType().Name}.");
                command.Process(client);
            }
            catch(EndOfStreamException)
            {
                // Client possibly disconnected...
            }
        }
    }
}