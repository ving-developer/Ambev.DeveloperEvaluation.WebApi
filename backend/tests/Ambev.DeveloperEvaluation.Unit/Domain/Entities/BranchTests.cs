using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class BranchTests
{
    [Fact(DisplayName = "Given valid data, When creating branch, Then properties are set")]
    public void Given_ValidData_When_CreatingBranch_Then_PropertiesSet()
    {
        var branch = BranchTestData.GenerateValidBranch();

        Assert.False(string.IsNullOrWhiteSpace(branch.Name));
        Assert.False(string.IsNullOrWhiteSpace(branch.Code));
        Assert.False(string.IsNullOrWhiteSpace(branch.City));
        Assert.False(string.IsNullOrWhiteSpace(branch.State));
        Assert.NotNull(branch.Sales);
        Assert.Empty(branch.Sales);
    }

    [Fact(DisplayName = "Given null name, When creating branch, Then throws ArgumentNullException")]
    public void Given_NullName_When_CreatingBranch_Then_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Branch(null!, "CODE01", "City", "ST"));
    }

    [Fact(DisplayName = "Given null code, When creating branch, Then throws ArgumentNullException")]
    public void Given_NullCode_When_CreatingBranch_Then_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Branch("BranchName", null!, "City", "ST"));
    }

    [Fact(DisplayName = "Given null city, When creating branch, Then throws ArgumentNullException")]
    public void Given_NullCity_When_CreatingBranch_Then_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Branch("BranchName", "CODE01", null!, "ST"));
    }

    [Fact(DisplayName = "Given null state, When creating branch, Then throws ArgumentNullException")]
    public void Given_NullState_When_CreatingBranch_Then_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Branch("BranchName", "CODE01", "City", null!));
    }

    [Fact(DisplayName = "Given branch, When adding carts to internal list, Then sales collection updated")]
    public void Given_Branch_When_AddingSales_Then_SalesCollectionUpdated()
    {
        // Guiven
        var branch = BranchTestData.GenerateValidBranch();
        var branchId = Guid.NewGuid();
        typeof(Branch).GetProperty("Id")!.SetValue(branch, branchId);

        var cart1 = new Cart(Guid.NewGuid(), branchId, "SALE001");
        var cart2 = new Cart(Guid.NewGuid(), branchId, "SALE002");
        var salesField = typeof(Branch).GetField("_sales", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var salesList = (System.Collections.IList)salesField!.GetValue(branch)!;

        // When
        salesList.Add(cart1);
        salesList.Add(cart2);

        // Then
        Assert.Equal(2, branch.Sales.Count);
        Assert.Contains(cart1, branch.Sales);
        Assert.Contains(cart2, branch.Sales);
    }

}
