import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getImageById } from "../service/imageService";
import type { ImageModel } from "../Model/ImageModel";

const ImageViewPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [image, setImage] = useState<ImageModel | null>(null);

  useEffect(() => {
    // console.log("ImageViewPage mounted with id:", id);
    if (!id) return;

    const fetchImage = async () => {
      try {
        const data = await getImageById(id);
        setImage(data);
      } catch (error) {
        console.error(error);
        alert("Image not found");
      }
    };

    fetchImage();
  }, [id]);

  return (
    <div style={{ textAlign: "center", padding: "20px" }}>
      {image ? (
        <img
          src={`https://localhost:7249${image.imageUrl}`}
          alt="Uploaded"
          style={{ maxWidth: "80%" }}
        />
      ) : (
        <p>Loading image...</p>
      )}
    </div>
  );
};

export default ImageViewPage;
