using System;

namespace generation {

public abstract class AbstractGenerator {
    private readonly Random random;

    protected AbstractGenerator() {
        random = new Random();
    }

    protected AbstractGenerator(int seed) {
        random = new Random(seed);
    }
}
}