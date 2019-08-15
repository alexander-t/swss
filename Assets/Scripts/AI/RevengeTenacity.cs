namespace AI
{
    /*
     * Determines how the AI responds when being fired at. Persistent enemies hunt the shooter until the shooter is dead or they die themselves.
     * Flaky enemies attack the owner of the last hitting beam.
     */
    public enum RevengeTenacity
    {
        Persistent, Flaky
    }
}
