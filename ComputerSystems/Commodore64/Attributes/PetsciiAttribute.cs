using System;

namespace Commodore64.Attributes {
    public class PetsciiAttribute : Attribute {
        public byte PetsciiCode { get; set; }
        public byte AsciiCode { get; set; }
        public string KeyCombination { get; set; }
        public string Description { get; set; }
    }
}
