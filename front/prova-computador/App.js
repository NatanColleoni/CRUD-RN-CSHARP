
import React, { useState, useEffect } from 'react';
import { Text, FlatList, TouchableOpacity, TextInput, StyleSheet, SafeAreaView, ScrollView } from 'react-native';
import ComputadorService from './services/ComputadorService';

export default function App() {
  const [computadores, setComputadores] = useState([]);
  const [selectedComputador, setSelectedComputador] = useState(null);
  const [nome, setNome] = useState('');
  const [cor, setCor] = useState('');
  const [dataFabricacao, setDataFabricacao] = useState(new Date());
  const [perifericos, setPerifericos] = useState([]);

  useEffect(async () => {
    await new ComputadorService().getAllComputadores().then(response => {
      console.log(response)
      setComputadores(response);
    });
  }, []);

  const handleCreateComputador = async () => {
    const computadorDto = {
      nome: nome,
      cor: cor,
      dataFabricacao: '2024-09-17',
      perifericos: perifericos
    };
    await new ComputadorService().createComputador(computadorDto)
    const novaListaComputadores = await new ComputadorService().getAllComputadores();
    setComputadores([]);
    setComputadores(...novaListaComputadores);
    setNome('');
    setCor('');
    setDataFabricacao(new Date());
    setPerifericos([]);
  };

  const handleUpdateComputador = async () => {
    if (!selectedComputador) return;
    const computadorDto = {
      ...selectedComputador,
      nome: nome,
      cor: cor,
      dataFabricacao: dataFabricacao,
      perifericos: perifericos
    };
    setComputadores(computadores.map(c => c.Id === selectedComputador.Id ? computadorDto : c));
    setSelectedComputador(null);
    setNome('');
    setCor('');
    setDataFabricacao(new Date());
    setPerifericos([]);
  };

  const handleDeleteComputador = async () => {
    if (!selectedComputador) return;
    await new ComputadorService().deleteComputador(selectedComputador.id);
    setComputadores(computadores.filter(c => c.id !== selectedComputador.id));
    setSelectedComputador(null);
  };

  const handleSelectComputador = (computador) => {
    setSelectedComputador(computador);
    setNome(computador.nome);
    setCor(computador.cor);
    setDataFabricacao(computador.dataFabricacao);
    setPerifericos(...computador.perifericos);
  }

  return (
    <SafeAreaView style={styles.formContainer}>
      <FlatList
        data={computadores}
        renderItem={({ item }) => (
          <TouchableOpacity onPress={() => handleSelectComputador(item)}>
            <Text>{item.nome}</Text>
          </TouchableOpacity>
        )}
        keyExtractor={item => item.id}
      />
      <ScrollView>
        <TextInput
          style={styles.input}
          placeholder="Nome"
          value={nome}
          onChangeText={text => setNome(text)}
        />
        <TextInput
          style={styles.input}
          placeholder="Cor"
          value={cor}
          onChangeText={text => setCor(text)}
        />
        <TextInput
          style={styles.input}
          placeholder="Data de Fabricação"
          value={dataFabricacao}
          onChangeText={text => setDataFabricacao(new Date(text))}
        />
        <TextInput
          editable
          style={styles.input}
          placeholder="Periféricos"
          value={perifericos}
          onChangeText={text => setPerifericos(text.split(','))}
        />
        {selectedComputador ? (
          <TouchableOpacity onPress={handleUpdateComputador}>
            <Text>Atualizar Computador</Text>
          </TouchableOpacity>
        ) : (
          <TouchableOpacity onPress={handleCreateComputador}>
            <Text>Criar Computador</Text>
          </TouchableOpacity>
        )}
      </ScrollView>
      {selectedComputador && (
        <TouchableOpacity onPress={handleDeleteComputador}>
          <Text>Deletar Computador</Text>
        </TouchableOpacity>
      )}
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  formContainer: {
    flex: 1,
    backgroundColor: '#f9f9f9',
    alignItems: 'center',
  },
  input: {
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    padding: 10,
    marginBottom: 20
  }
});
