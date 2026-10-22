using FluentMigrator;
using System.Data;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.RECIPES_TABLE, "Creating Recipe Table")]
public class Version2 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Recipes")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Title").AsString(250).NotNullable()
            .WithColumn("CookTime").AsInt32().NotNullable()
            .WithColumn("UserId").AsGuid().NotNullable()
                .ForeignKey("FK_Recipes_Users_UserId", "Users", "Id"); //Sem onDelete pq se deletar o user a receita seria deletada tb 

        Create.Table("RecipeIngredients")
            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Item").AsString(250).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable()
                .ForeignKey("FK_RecipeIngredients_Recipes_RecipeId", "Recipes", "Id").OnDelete(Rule.Cascade);//"Se deletar uma receita deleta os ingredientes, instrucoes etc em cascata"

        Create.Table("RecipeIntructions")
            .WithColumn("Id").AsGuid().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Order").AsInt32().NotNullable()
            .WithColumn("Description").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable()
                .ForeignKey("FK_RecipeInstructions_Recipes_RecipeId", "Recipes", "Id").OnDelete(Rule.Cascade);

        Create.Table("RecipeDishTypes")
            .WithColumn("Id").AsGuid().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("RecipeId").AsGuid().NotNullable()
                .ForeignKey("FK_RecipeDishTypes_Recipes_RecipeId", "Recipes", "Id").OnDelete(Rule.Cascade);
    }   
}
