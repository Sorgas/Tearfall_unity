namespace types.action {

    /**
     * Enumeration of {@link Action} check results.
     * Actions are checked by {@link CreatureActionPerformingSystem}.
     *
     * @author Alexander on 30.12.2019.
     */
    public enum ActionCheckingEnum {
        OK, // if checked successfully.
        NEW, // if new sub action created and added to task.
        FAIL // if unable perform action or create sub action.
    }
}