namespace types.unit.health {
    public class GameplayStatEnum {
        // public static readonly GameplayStat MOVEMENT_SPEED = new GameplayStat(3, health -> health.functions.get(WALKING) * health.functions.get(CONSCIOUSNESS), Arrays.asList(WALKING, CONSCIOUSNESS), null),
        // public static readonly GameplayStat ATTACK_SPEED = new GameplayStat(1, health -> (1 + 0.03f * health.attributes.get(AGILITY)) * health.functions.get(MOTORIC) * health.functions.get(CONSCIOUSNESS), Arrays.asList(MOTORIC, CONSCIOUSNESS), Arrays.asList(AGILITY)),
        // public static readonly GameplayStat WORK_SPEED = new GameplayStat(1, health -> health.functions.get(CONSCIOUSNESS) * health.functions.get(MOTORIC), Arrays.asList(MOTORIC, CONSCIOUSNESS), null);
        // // should be multiplied by skill for each action
        //
        // public static final Map<String, GameplayStatEnum> map = new HashMap<>();
        // public static final Map<HealthFunctionEnum, Set<GameplayStatEnum>> functionsToStats = new HashMap<>();
        // public static final Map<CreatureAttributeEnum, Set<GameplayStatEnum>> attributesToStats = new HashMap<>();
        //
        // static {
        //     Arrays.stream(values()).forEach(stat -> {
        //         map.put(stat.toString().toLowerCase(), stat);
        //         stat.functions.stream()
        //                 .map(function -> functionsToStats.computeIfAbsent(function, func -> new HashSet<>()))
        //                 .forEach(set -> set.add(stat));
        //         stat.attributes.stream()
        //                 .map(attribute -> attributesToStats.computeIfAbsent(attribute, attr -> new HashSet<>()))
        //                 .forEach(set -> set.add(stat));
        //     });
        // }
        //
        // GameplayStatEnum(float humanDefault, Function<HealthAspect, Float> function, List<HealthFunctionEnum> functions, List<CreatureAttributeEnum> attributes) {
        //     DEFAULT = humanDefault;
        //     FUNCTION = function;
        //     this.functions = functions == null ? Collections.emptyList() : functions;
        //     this.attributes = attributes == null ? Collections.emptyList() : attributes;
        // }
        //
        // public static Set<GameplayStatEnum> collectProperties(HealthEffect effect) {
        //     return Stream.concat(
        //             effect.functionEffects.keySet().stream().map(functionsToStats::get),
        //             effect.attributeEffects.keySet().stream().map(attributesToStats::get)
        //     )
        //             .filter(Objects::nonNull)
        //             .flatMap(Set::stream)
        //             .collect(Collectors.toSet());
        // }
    }

    public class GameplayStat {
        // public readonly float DEFAULT;
        // public readonly Func<HealthAspect, float> FUNCTION;
        // private readonly List<HealthFunctionEnum> functions;
        // private readonly List<CreatureAttributeEnum> attributes;
        //
        // public GameplayStat(float @default, Func<HealthAspect, float> function, List<HealthFunctionEnum> functions, List<CreatureAttributeEnum> attributes) {
        //     DEFAULT = @default;
        //     FUNCTION = function;
        //     this.functions = functions;
        //     this.attributes = attributes;
        // }
    }
}