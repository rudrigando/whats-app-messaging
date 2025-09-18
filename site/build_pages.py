#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Script para gerar páginas HTML usando o sistema de layout
"""

import os
import re

def build_page(layout_path, config_path, output_path):
    """
    Constrói uma página HTML usando o layout base e configuração específica
    """
    print(f"🔨 Construindo página: {output_path}")
    
    # Ler o layout base
    with open(layout_path, 'r', encoding='utf-8') as f:
        layout_content = f.read()
    
    # Ler a configuração da página
    with open(config_path, 'r', encoding='utf-8') as f:
        config_content = f.read()
    
    # Extrair configurações usando regex
    placeholders = {
        "{{PAGE_TITLE}}": "",
        "{{NAVIGATION}}": "",
        "{{PAGE_CONTENT}}": "",
        "{{PAGE_STYLES}}": "",
        "{{PAGE_SCRIPTS}}": ""
    }
    
    # Processar cada seção da configuração
    current_placeholder = None
    current_content = []
    
    for line in config_content.splitlines():
        # Verificar se é um marcador de seção (sem strip para preservar indentação)
        if line.strip().startswith("<!-- {{") and line.strip().endswith("}} -->"):
            # Salvar conteúdo anterior se houver
            if current_placeholder and current_content:
                placeholders[current_placeholder] = "\n".join(current_content)
            
            # Iniciar nova seção
            current_placeholder = line.strip("<!-- ").strip(" -->")
            current_content = []
        else:
            # Adicionar linha ao conteúdo atual (preservando indentação)
            if current_placeholder:
                current_content.append(line)
    
    # Salvar última seção
    if current_placeholder and current_content:
        placeholders[current_placeholder] = "\n".join(current_content)
    
    # Substituir placeholders no layout
    for placeholder, content in placeholders.items():
        layout_content = layout_content.replace(placeholder, content.strip())
    
    # Escrever arquivo final
    with open(output_path, 'w', encoding='utf-8') as f:
        f.write(layout_content)
    
    print(f"✅ Página construída: {output_path}")

def main():
    """
    Função principal para construir todas as páginas
    """
    print("🚀 Iniciando construção das páginas HTML...")
    
    # Verificar se os arquivos necessários existem
    required_files = ['layout.html', 'index_config.html', 'documentacao_config.html']
    for file in required_files:
        if not os.path.exists(file):
            print(f"❌ Arquivo necessário não encontrado: {file}")
            return
    
    try:
        # Construir index.html
        build_page('layout.html', 'index_config.html', 'index.html')
        
        # Construir documentacao.html
        build_page('layout.html', 'documentacao_config.html', 'documentacao.html')
        
        print("\n✨ Todas as páginas foram construídas com sucesso!")
        print("📁 Arquivos gerados:")
        print("   - index.html")
        print("   - documentacao.html")
        print("\n🎯 Header e footer estão centralizados no layout.html")
        
    except Exception as e:
        print(f"❌ Erro durante a construção: {e}")

if __name__ == "__main__":
    main()
