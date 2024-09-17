import axios from 'axios';

export default class ComputadorService {
  defaultUrl = "https://localhost:7160";
  
  async getAllComputadores() {
    const response = await axios.get(`${this.defaultUrl}/computadores`)
    return response.data
  }
  
  async createComputador(computadorDto) {
    const response = await axios.post(`${this.defaultUrl}/computadores`, JSON.stringify(computadorDto),{
      headers: {
        'Content-Type': 'application/json'
      }
    });
    return response.data;
  }

  async updateComputador(id, computadorDto) {
    const response = await axios.put(`${this.defaultUrl}/computadores/${id}`, {
      Nome: computadorDto.Nome,
      Cor: computadorDto.Cor,
      DataFabricacao: computadorDto.DataFabricacao,
      Perifericos: computadorDto.Perifericos
    });
    return response.data;
  }

  async deleteComputador(id) {
    const response = await axios.delete(`${this.defaultUrl}/computadores/${id}`)
    return response.data
  }
}