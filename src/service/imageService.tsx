import axios from "axios";
import type { ImageModel } from "../Model/ImageModel";

const API_BASE_URL = "https://localhost:7249/api"; //192.168.1.5:5206/api/values/upload https://localhost:7249/api/values/upload

export const uploadImage = async (imageFile: File) => {
  const formData = new FormData();
  formData.append("imageFile", imageFile);

  const response = await axios.post(`${API_BASE_URL}/values/upload`, formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });

  return response.data as {
    imageId: string;
    imageUrl: string;
  };
};

export const getImageById = async (id: string): Promise<ImageModel> => {
  const response = await axios.get(
    `${API_BASE_URL}/values/getbyid/${id}`
  );
  return response.data; // TypeScript now knows this is ImageModel
};
