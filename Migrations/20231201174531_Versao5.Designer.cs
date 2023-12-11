﻿// <auto-generated />
using System;
using EstoqueWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EstoqueWeb.Migrations
{
    [DbContext(typeof(EstoqueWebContext))]
    [Migration("20231201174531_Versao5")]
    partial class Versao5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.24");

            modelBuilder.Entity("EstoqueWeb.Models.CategoriaModel", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("IdCategoria");

                    b.ToTable("Categoria", (string)null);
                });

            modelBuilder.Entity("EstoqueWeb.Models.ClienteModel", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("char(14)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("IdUsuario");

                    b.ToTable("Cliente");
                });

            modelBuilder.Entity("EstoqueWeb.Models.ProdutoModel", b =>
                {
                    b.Property<int>("IdProduto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Estoque")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdCategoria")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<double>("Preco")
                        .HasColumnType("REAL");

                    b.HasKey("IdProduto");

                    b.HasIndex("IdCategoria");

                    b.ToTable("Produto", (string)null);
                });

            modelBuilder.Entity("EstoqueWeb.Models.ClienteModel", b =>
                {
                    b.OwnsMany("EstoqueWeb.Models.EnderecoModel", "Enderecos", b1 =>
                        {
                            b1.Property<int>("IdUsuario")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("IdEndereco")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Bairro")
                                .HasColumnType("TEXT");

                            b1.Property<string>("CEP")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Cidade")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Complemento")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Estado")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Logradouro")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Numero")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Referencia")
                                .HasColumnType("TEXT");

                            b1.HasKey("IdUsuario", "IdEndereco");

                            b1.ToTable("Endereco");

                            b1.WithOwner()
                                .HasForeignKey("IdUsuario");
                        });

                    b.Navigation("Enderecos");
                });

            modelBuilder.Entity("EstoqueWeb.Models.ProdutoModel", b =>
                {
                    b.HasOne("EstoqueWeb.Models.CategoriaModel", "Categoria")
                        .WithMany("Produtos")
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("EstoqueWeb.Models.CategoriaModel", b =>
                {
                    b.Navigation("Produtos");
                });
#pragma warning restore 612, 618
        }
    }
}
