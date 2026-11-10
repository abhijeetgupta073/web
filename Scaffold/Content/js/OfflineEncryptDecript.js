//function OffEncrypt(text) {
//    const key = CryptoJS.enc.Utf8.parse('KJHSGFHKJS4578DSJ545');
//    const iv = CryptoJS.lib.WordArray.random(16);
//    const encrypted = CryptoJS.AES.encrypt(text, key, {
//        iv: iv,
//        mode: CryptoJS.mode.CBC,
//        padding: CryptoJS.pad.Pkcs7
//    });

//    return iv.toString() + ':' + encrypted.toString();
//}

//function OffDecrypt(encryptedText) {
//    const key = CryptoJS.enc.Utf8.parse('KJHSGFHKJS4578DSJ545');
//    const parts = encryptedText.split(':');
//    const iv = CryptoJS.enc.Hex.parse(parts[0]);
//    const encrypted = parts[1];

//    const decrypted = CryptoJS.AES.decrypt(encrypted, key, {
//        iv: iv,
//        mode: CryptoJS.mode.CBC,
//        padding: CryptoJS.pad.Pkcs7
//    });

//    return decrypted.toString(CryptoJS.enc.Utf8);
//} 
function OffEncrypt(text) {
    // Use a valid 16-byte (128-bit) AES key
    const key = CryptoJS.enc.Utf8.parse('KJHSGFHKJS4578D5'); // 16 characters = 128 bits
    // Generate a random 16-byte (128-bit) IV
    const iv = CryptoJS.lib.WordArray.random(16);

    // Encrypt the text
    const encrypted = CryptoJS.AES.encrypt(text, key, {
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    // Return the IV and encrypted text as a single string (IV:ciphertext)
    return iv.toString(CryptoJS.enc.Hex) + ':' + encrypted.ciphertext.toString(CryptoJS.enc.Base64);
}

function OffDecrypt(encryptedText) {
    // Use the same AES key as in encryption
    const key = CryptoJS.enc.Utf8.parse('KJHSGFHKJS4578D5'); // 16 characters = 128 bits

    // Split the input into IV and ciphertext
    const parts = encryptedText.split(':');
    if (parts.length !== 2) {
        throw new Error('Invalid encrypted text format');
    }

    const iv = CryptoJS.enc.Hex.parse(parts[0]); // Parse the IV from hex
    const ciphertext = CryptoJS.enc.Base64.parse(parts[1]); // Parse the ciphertext from Base64

    // Decrypt the ciphertext
    const decrypted = CryptoJS.AES.decrypt({ ciphertext: ciphertext }, key, {
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    // Convert the decrypted data to a UTF-8 string
    return decrypted.toString(CryptoJS.enc.Utf8);
}
