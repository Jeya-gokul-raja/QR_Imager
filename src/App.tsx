
import { BrowserRouter, Route, Routes } from "react-router-dom";
import ImageUpload from "./component/Navbar";
import ImageViewPage from "./component/ImagePage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<ImageUpload />} />
        <Route path="/view-image/:id" element={<ImageViewPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
