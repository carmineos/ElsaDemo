﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Onboarding.Data;

#nullable disable

namespace Onboarding.Migrations
{
    [DbContext(typeof(OnboardingDbContext))]
    [Migration("20240903091627_RenameTaskType")]
    partial class RenameTaskType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Onboarding.Data.Models.Workflows.TaskRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("CompletedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CompletedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalTaskId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TaskTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("WorkflowRequestId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TaskTypeId");

                    b.HasIndex("WorkflowRequestId");

                    b.ToTable("TaskRequests");
                });

            modelBuilder.Entity("Onboarding.Data.Models.Workflows.TaskType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TaskTypes");
                });

            modelBuilder.Entity("Onboarding.Data.Models.Workflows.WorkflowRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CompletedAtUtc")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAtUtc")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestJsonData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RequestorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkflowInstanceId")
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkflowTemplateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowTemplateId");

                    b.ToTable("WorkflowRequests");
                });

            modelBuilder.Entity("Onboarding.Data.Models.Workflows.WorkflowTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RequestJsonSchema")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkflowDefinitionId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkflowTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowTypeId");

                    b.ToTable("WorkflowTemplates");
                });

            modelBuilder.Entity("Onboarding.Data.Models.Workflows.WorkflowType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WorkflowTypes");
                });

            modelBuilder.Entity("Onboarding.Data.Models.Workflows.TaskRequest", b =>
                {
                    b.HasOne("Onboarding.Data.Models.Workflows.TaskType", "TaskType")
                        .WithMany()
                        .HasForeignKey("TaskTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Onboarding.Data.Models.Workflows.WorkflowRequest", "WorkflowRequest")
                        .WithMany("TaskRequests")
                        .HasForeignKey("WorkflowRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TaskType");

                    b.Navigation("WorkflowRequest");
                });

            modelBuilder.Entity("Onboarding.Data.Models.Workflows.WorkflowRequest", b =>
                {
                    b.HasOne("Onboarding.Data.Models.Workflows.WorkflowTemplate", "WorkflowTemplate")
                        .WithMany()
                        .HasForeignKey("WorkflowTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkflowTemplate");
                });

            modelBuilder.Entity("Onboarding.Data.Models.Workflows.WorkflowTemplate", b =>
                {
                    b.HasOne("Onboarding.Data.Models.Workflows.WorkflowType", "WorkflowType")
                        .WithMany()
                        .HasForeignKey("WorkflowTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkflowType");
                });

            modelBuilder.Entity("Onboarding.Data.Models.Workflows.WorkflowRequest", b =>
                {
                    b.Navigation("TaskRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
