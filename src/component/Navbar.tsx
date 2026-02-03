import React, { useState } from "react";
import { QRCodeCanvas } from "qrcode.react";
import { uploadImage } from "../service/imageService"; // corrected path

const ImageUpload: React.FC = () => {
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string>("");
  const [imageId, setImageId] = useState<string>("");
  const [qrValue, setQrValue] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    setImageFile(file);
    setPreviewUrl(URL.createObjectURL(file)); // ⭐ magic line
  };
  const handleUpload = async () => {
    if (!imageFile) {
      alert("Please select an image");
      return;
    }

    try {
      const data = await uploadImage(imageFile); // returns { imageId, imageUrl }

      setImageId(data.imageId); // store the id
      // Generate QR code that points to the frontend page

      setQrValue(`http://localhost:5173/view-image/${data.imageId}`);
    } catch (error) {
      console.error(error);
      alert("Image upload failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Upload Image & Generate QR Code</h2>

      <input
        type="file"
        className="border"
        accept="image/*"
        onChange={handleFileChange}
      />

      <br />
      <br />

      <button onClick={handleUpload} disabled={loading}>
        {loading ? "Uploading..." : "Upload Image"}
      </button>

      {previewUrl && (
        <div style={{ marginTop: "15px" }}>
          <p>
            <b>Preview (before upload)</b>
          </p>
          <img
            src={previewUrl}
            alt="Preview"
            style={{ maxWidth: "300px", borderRadius: "8px" }}
          />
        </div>
      )}

      {imageId && (
        <p>
          <b>Image ID:</b> {imageId}
        </p>
      )}

      {qrValue && (
        <div style={{ marginTop: "20px" }}>
          <QRCodeCanvas value={qrValue} size={200} />
        </div>
      )}
    </div>
  );
};

export default ImageUpload;
