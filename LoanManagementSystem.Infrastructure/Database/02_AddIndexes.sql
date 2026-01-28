USE lmsdb;

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_LoanApplications_ClientId')
BEGIN
	CREATE INDEX IX_LoanApplications_ClientId ON LoanApplications(ClientId);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_RepaymentSchedules_LoanApplicationId')
BEGIN
	CREATE INDEX IX_RepaymentSchedules_LoanApplicationId ON LoanApplications(ClientId);
END