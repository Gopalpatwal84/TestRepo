namespace PowerBI.Jobs.DocumentationFeedback.Data
{
    using System;
    using System.Linq;
    using Context;
    using PowerBI.Jobs.DocumentationFeedback.Data.Models;

    public class DocumentationFeedbackDbDataService
    {
        private static readonly object LockObject = new object();

        private IDocumentationFeedbackContext context;

        public DocumentationFeedbackDbDataService(IDocumentationFeedbackContext context)
        {
            this.context = context;
        }

        public void SaveFeedback(Feedback feedback)
        {
            try
            {
                lock (LockObject)
                {
                    var relatedFeedback = this.GetFeedback(feedback.FeedbackId);

                    if (relatedFeedback == null)
                    {
                        this.context.Feedback.Add(feedback);
                        this.context.SaveChanges();
                    }
                    else if (string.IsNullOrWhiteSpace(relatedFeedback.Comment))
                    {
                        relatedFeedback.Comment = feedback.Comment;
                        this.context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("There's been an error trying to save a feedback message '{0}' to the database. Details: {1}", feedback.FeedbackId, e.Message), e);
            }
        }

        public Feedback GetFeedback(Guid feedbackId)
        {
            try
            {
                return this.context.Feedback.FirstOrDefault(f => f.FeedbackId == feedbackId);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("There's been an error trying to obtain the feedback message '{0}' from the database. Details: {1}", feedbackId, e.Message), e);
            }
        }

        public void DeleteFeedback(Feedback feedback)
        {
            try
            {
                this.context.Feedback.Remove(feedback);

                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("There's been an error trying to delete the feedback message '{0}' from the database. Details: {0}", feedback.FeedbackId, e.Message), e);
            }
        }
    }
}
