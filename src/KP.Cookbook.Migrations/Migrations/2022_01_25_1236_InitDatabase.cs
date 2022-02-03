using SimpleMigrations;

namespace KP.Cookbook.Migrations.Migrations
{
    [Migration(202201251236, "Initial database creation")]
    public class _2022_01_25_1236_InitDatabase : Migration
    {
        protected override void Up()
        {
            Execute(@"
				CREATE TABLE ingredients
				(
					id 			bigint 		GENERATED ALWAYS AS IDENTITY 	PRIMARY KEY,
					name 		text		NOT NULL,
					type 		smallint	NOT NULL,
					description text		NULL
				);

				CREATE TABLE users
				(
					id 				bigint 		GENERATED ALWAYS AS IDENTITY 	PRIMARY KEY,
					nickname 		text		NULL,
					avatar			text 		NULL,
					type			smallint 	NOT NULL,
					joined_at		timestamp	NOT NULL,
					login			text		NOT NULL,
					password_hash 	text		NOT NULL
				);

				CREATE TABLE sources
				(
					id 				bigint 		GENERATED ALWAYS AS IDENTITY 	PRIMARY KEY,
					name 			text		NOT NULL,
					description		text 		NULL,
					link			text 		NULL,
					image			text 		NULL,
					is_approved		boolean		NOT NULL
				);

				CREATE TABLE cooking_steps
				(
					id 			bigint 		GENERATED ALWAYS AS IDENTITY 	PRIMARY KEY,
					_order		smallint	NOT NULL,
					description	text		NULL,
					image		text		NULL
				);

                CREATE TABLE cooking_steps_and_ingredients
                (
	                cooking_step_id		bigint			REFERENCES cooking_steps (id) 	ON UPDATE CASCADE ON DELETE CASCADE,
	                ingredient_id		bigint			REFERENCES ingredients (id) 	ON UPDATE CASCADE ON DELETE CASCADE,
	                amount				numeric(6, 2) 	NOT NULL,
	                amount_type			smallint		NOT NULL,
	                is_optional			boolean			NOT NULL,
	
	                CONSTRAINT cooking_steps_and_ingredients_pk PRIMARY KEY (cooking_step_id, ingredient_id)
                );

				CREATE TABLE recipes
				(
					id 				bigint 		GENERATED ALWAYS AS IDENTITY 	PRIMARY KEY,
					title			text		NOT NULL,
					recipe_type 	smallint	NOT NULL,
					cooking_type	smallint 	NOT NULL,
					kitchen_type	smallint	NOT NULL,
					holiday_type	smallint	NOT NULL,
					created_at		timestamp	NOT NULL,	
					user_id			bigint		REFERENCES users(id) ON DELETE CASCADE			NOT NULL,	
					source_id		bigint		REFERENCES sources(id) ON DELETE SET NULL		NULL,
					duration_min	smallint	NULL,
					description		text		NULL,
					image			text		NULL,
					updated_at		timestamp	NULL
				);

				CREATE INDEX recipes_title_index ON recipes USING btree (title);

				CREATE TABLE recipes_and_ingredients
				(
					recipe_id			bigint			REFERENCES recipes (id) 	    ON UPDATE CASCADE ON DELETE CASCADE,
					ingredient_id		bigint			REFERENCES ingredients (id) 	ON UPDATE CASCADE ON DELETE CASCADE,
					amount				numeric(6, 2) 	NOT NULL,
					amount_type			smallint		NOT NULL,
					is_optional			boolean			NOT NULL,

					CONSTRAINT recipes_and_ingredients_pk PRIMARY KEY (recipe_id, ingredient_id)
				);

				CREATE TABLE recipes_and_cooking_steps
				(
					recipe_id			bigint			REFERENCES recipes (id) 		ON UPDATE CASCADE ON DELETE CASCADE,
					cooking_step_id		bigint			REFERENCES cooking_steps (id) 	ON UPDATE CASCADE ON DELETE CASCADE,

					CONSTRAINT recipes_and_cooking_steps_pk PRIMARY KEY (recipe_id, cooking_step_id)
				);
            ");
        }

        protected override void Down() =>
            Execute(@"
                DROP TABLE ingredients;
                DROP TABLE users;
                DROP TABLE sources;
                DROP TABLE cooking_steps;
                DROP TABLE cooking_steps_and_ingredients;

				DROP TABLE recipes;
				DROP TABLE recipes_and_ingredients;
				DROP TABLE recipes_and_cooking_steps;
            ");
    }
}
