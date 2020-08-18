using Exceptions;
using Microsoft.Office.Interop.Excel;
using Rules.Service.Interfaces;
using Rules.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Range = Microsoft.Office.Interop.Excel.Range;


namespace Rules.Service
{
    public class RuleService : IRuleService
    {
        const int RESOURCES_WORKSHEET = 1;
        private readonly IRuleRepository _rulesRepository;

        public RuleService(IRuleRepository rulesRepository)
        {
            _rulesRepository = rulesRepository;
        }

        public async Task UpdatePolicyRules(RegisterModel registerModel)
        {
            bool isProviderLoanExist = await _rulesRepository.CheckProviderLoanExist(registerModel.ProviderLoanId);
            if (isProviderLoanExist)
            {
                List<RuleModel> policyRules = ConvertExcelToRulesObject(registerModel.FilePath);
                List<RuleModel> rulesListToDb = CreateRulesTree(policyRules, registerModel.ProviderLoanId);
                await _rulesRepository.UpdatePolicy(registerModel.ProviderLoanId, rulesListToDb);
                
            }
            else
            {
                //האם לזרוק שגיאה של לא נמצא או להפנות לרישום
                //וכן בסרויס ההלוואות בעדכון הלוואה
                throw new LoanProviderNotFoundException();
            }
        }

        public async Task RegisterToProvideLoans(RegisterModel registerModel)
        {                                 
            bool isProviderLoanExist = await _rulesRepository.CheckProviderLoanExist(registerModel.ProviderLoanId);
            if (isProviderLoanExist)
            {
                throw new DuplicateLoanProviderIdException();
            }
            else
            {
                List<RuleModel> policyRules = ConvertExcelToRulesObject(registerModel.FilePath);
                List<RuleModel> rulesListToDb = CreateRulesTree(policyRules, registerModel.ProviderLoanId);
                await _rulesRepository.CreatePolicy(rulesListToDb);
            }
        }

        private List<RuleModel> ConvertExcelToRulesObject(string filePath)
        {           
                Application xlApp;
                Workbook xlWorkBook;
                Worksheet xlWorkSheet;
                Range range;

                int rCnt;
                int rw = 0;
                int cl = 0;

                xlApp = new Application();
                //open the excel
                xlWorkBook = xlApp.Workbooks.Open(filePath);
                //get the first sheet of the excel
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

                range = xlWorkSheet.UsedRange;
                // get the total row count
                rw = range.Rows.Count;
                //get the total column count
                cl = range.Columns.Count;

                List<RuleModel> rules = new List<RuleModel>();
                // traverse all the row in the excel not inclde titles                
                for (rCnt = 2; rCnt <= rw; rCnt++)
                {                    
                    RuleModel rule = new RuleModel();
                //Func create rule
                    // get the value of the columns in the current row
                    rule.Id = (int)(range.Cells[rCnt, 1] as Range).Value2;
                    rule.ValueToCompeare = (range.Cells[rCnt, 2] as Range).Value2.ToString();
                    rule.Operator = (range.Cells[rCnt, 3] as Range).Value2.ToString();
                    rule.Parameter = (range.Cells[rCnt, 4] as Range).Value2.ToString();
                    rule.ParentId = (int)(range.Cells[rCnt, 5] as Range).Value2;
                    rules.Add(rule);
                }

                //release the resources
                xlWorkBook.Close(true, null, null);
                xlApp.Quit();
                Marshal.ReleaseComObject(xlWorkSheet);
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
                return rules;            
        }

        private List<RuleModel> CreateRulesTree(List<RuleModel> policyRules, Guid loanProviderId)
        {
            var childsHash = policyRules.ToLookup(rule => rule.ParentId);
            foreach (var rule in policyRules)
            {
                rule.ChildrenRules = childsHash[rule.Id].ToList();
                rule.LoanProviderId = loanProviderId;
            }
            return policyRules;
        }        
    }
}
