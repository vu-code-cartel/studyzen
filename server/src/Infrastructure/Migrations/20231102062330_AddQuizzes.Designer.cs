﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudyZen.Infrastructure.Persistence;

#nullable disable

namespace StudyZen.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231102062330_AddQuizzes")]
    partial class AddQuizzes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudyZen.Domain.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.Flashcard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Back")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("FlashcardSetId")
                        .HasColumnType("int");

                    b.Property<string>("Front")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("FlashcardSetId");

                    b.ToTable("Flashcards");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.FlashcardSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Color")
                        .HasColumnType("int");

                    b.Property<int?>("LectureId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("LectureId");

                    b.ToTable("FlashcardSets");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.Lecture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Lectures");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.Quiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.QuizAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("QuizQuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuizQuestionId");

                    b.ToTable("QuizAnswers");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.QuizQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CorrectAnswerId")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("QuizId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("TimeLimit")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("CorrectAnswerId")
                        .IsUnique()
                        .HasFilter("[CorrectAnswerId] IS NOT NULL");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.Course", b =>
                {
                    b.OwnsOne("StudyZen.Domain.ValueObjects.UserActionStamp", "CreatedBy", b1 =>
                        {
                            b1.Property<int>("CourseId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.Property<string>("User")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("CourseId");

                            b1.ToTable("Courses");

                            b1.WithOwner()
                                .HasForeignKey("CourseId");
                        });

                    b.OwnsOne("StudyZen.Domain.ValueObjects.UserActionStamp", "UpdatedBy", b1 =>
                        {
                            b1.Property<int>("CourseId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.Property<string>("User")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("CourseId");

                            b1.ToTable("Courses");

                            b1.WithOwner()
                                .HasForeignKey("CourseId");
                        });

                    b.Navigation("CreatedBy")
                        .IsRequired();

                    b.Navigation("UpdatedBy")
                        .IsRequired();
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.Flashcard", b =>
                {
                    b.HasOne("StudyZen.Domain.Entities.FlashcardSet", "FlashcardSet")
                        .WithMany("Flashcards")
                        .HasForeignKey("FlashcardSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FlashcardSet");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.FlashcardSet", b =>
                {
                    b.HasOne("StudyZen.Domain.Entities.Lecture", "Lecture")
                        .WithMany("FlashcardSets")
                        .HasForeignKey("LectureId");

                    b.OwnsOne("StudyZen.Domain.ValueObjects.UserActionStamp", "CreatedBy", b1 =>
                        {
                            b1.Property<int>("FlashcardSetId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.Property<string>("User")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("FlashcardSetId");

                            b1.ToTable("FlashcardSets");

                            b1.WithOwner()
                                .HasForeignKey("FlashcardSetId");
                        });

                    b.OwnsOne("StudyZen.Domain.ValueObjects.UserActionStamp", "UpdatedBy", b1 =>
                        {
                            b1.Property<int>("FlashcardSetId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.Property<string>("User")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("FlashcardSetId");

                            b1.ToTable("FlashcardSets");

                            b1.WithOwner()
                                .HasForeignKey("FlashcardSetId");
                        });

                    b.Navigation("CreatedBy")
                        .IsRequired();

                    b.Navigation("Lecture");

                    b.Navigation("UpdatedBy")
                        .IsRequired();
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.Lecture", b =>
                {
                    b.HasOne("StudyZen.Domain.Entities.Course", "Course")
                        .WithMany("Lectures")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("StudyZen.Domain.ValueObjects.UserActionStamp", "CreatedBy", b1 =>
                        {
                            b1.Property<int>("LectureId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.Property<string>("User")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LectureId");

                            b1.ToTable("Lectures");

                            b1.WithOwner()
                                .HasForeignKey("LectureId");
                        });

                    b.OwnsOne("StudyZen.Domain.ValueObjects.UserActionStamp", "UpdatedBy", b1 =>
                        {
                            b1.Property<int>("LectureId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.Property<string>("User")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LectureId");

                            b1.ToTable("Lectures");

                            b1.WithOwner()
                                .HasForeignKey("LectureId");
                        });

                    b.Navigation("Course");

                    b.Navigation("CreatedBy")
                        .IsRequired();

                    b.Navigation("UpdatedBy")
                        .IsRequired();
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.Quiz", b =>
                {
                    b.OwnsOne("StudyZen.Domain.ValueObjects.UserActionStamp", "CreatedBy", b1 =>
                        {
                            b1.Property<int>("QuizId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.Property<string>("User")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("QuizId");

                            b1.ToTable("Quizzes");

                            b1.WithOwner()
                                .HasForeignKey("QuizId");
                        });

                    b.OwnsOne("StudyZen.Domain.ValueObjects.UserActionStamp", "UpdatedBy", b1 =>
                        {
                            b1.Property<int>("QuizId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Timestamp")
                                .HasColumnType("datetime2");

                            b1.Property<string>("User")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("QuizId");

                            b1.ToTable("Quizzes");

                            b1.WithOwner()
                                .HasForeignKey("QuizId");
                        });

                    b.Navigation("CreatedBy")
                        .IsRequired();

                    b.Navigation("UpdatedBy")
                        .IsRequired();
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.QuizAnswer", b =>
                {
                    b.HasOne("StudyZen.Domain.Entities.QuizQuestion", null)
                        .WithMany("PossibleAnswers")
                        .HasForeignKey("QuizQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.QuizQuestion", b =>
                {
                    b.HasOne("StudyZen.Domain.Entities.QuizAnswer", "CorrectAnswer")
                        .WithOne()
                        .HasForeignKey("StudyZen.Domain.Entities.QuizQuestion", "CorrectAnswerId");

                    b.HasOne("StudyZen.Domain.Entities.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CorrectAnswer");

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.Course", b =>
                {
                    b.Navigation("Lectures");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.FlashcardSet", b =>
                {
                    b.Navigation("Flashcards");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.Lecture", b =>
                {
                    b.Navigation("FlashcardSets");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.Quiz", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("StudyZen.Domain.Entities.QuizQuestion", b =>
                {
                    b.Navigation("PossibleAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}
