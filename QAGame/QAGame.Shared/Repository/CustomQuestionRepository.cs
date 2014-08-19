using Microsoft.WindowsAzure.MobileServices;
using QAGame.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QAGame.Repository
{
    public class CustomQuestionRepository
    {
        private MobileServiceCollection<CustomQuestion, CustomQuestion> customQuestions;
        private MobileServiceClient mobileService;

        private IMobileServiceTable<CustomQuestion> customQuestionTable;
        public CustomQuestionRepository()
        {
            // Move this into app bootstrapper in future?
            mobileService = new MobileServiceClient("https://qagame.azure-mobile.net/", "pTztRKvKJOMdQPnlGFgfkbSQbMAdXo26");
            customQuestionTable = mobileService.GetTable<CustomQuestion>();
        }

        private async void addCustomQuestion(CustomQuestion customQuestionToAdd)
        {
            await customQuestionTable.InsertAsync(customQuestionToAdd);
        }

        public void Add(CustomQuestion customQuestionToAdd)
        {
            addCustomQuestion(customQuestionToAdd);
        }
    }
}
