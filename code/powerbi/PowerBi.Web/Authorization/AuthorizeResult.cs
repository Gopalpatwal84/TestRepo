namespace PowerBI.Web.Authorization
{
    public enum AuthorizeResult
    {
        /// <summary>
        /// deny and skip authorization of remanining agents
        /// </summary>
        Deny,

        /// <summary>
        /// deny but authorization continues and if next agents returns allow, user is allowed
        /// </summary>
        DenyConditional,

        /// <summary>
        /// allow and skip authorization of remanining agents
        /// </summary>
        Allow,

        /// <summary>
        /// user is allowed by this agent but has to continue with authorization through the rest of agents in the pipeline
        /// </summary>
        AllowConditional
    }
}