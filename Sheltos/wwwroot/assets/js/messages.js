function loadChat(name, subject, response) {
  const chatName = document.getElementById('chat-name');
  const chatBody = document.getElementById('chatBody');
  const contactBtn = document.getElementById('contactBtn');

  chatName.innerText = `${name} - ${subject}`;
  contactBtn.classList.remove('d-none');

  chatBody.innerHTML = `
    <div>
      <div class="mb-2"><strong>You:</strong> Is this still available?</div>
      ${response ? `<div class="mb-2"><strong>${name}:</strong> ${response}</div>` : ''}
      <div class="text-center mt-4 text-warning border p-2 rounded">
        ⚠️ Avoid paying in advance! Even for delivery
      </div>
    </div>
  `;
}


  function startChat() {
    const agentName = document.getElementById("agentName").innerText.trim();
    const message = document.getElementById("chatInput").value.trim();

    if (!message) {
      alert("Please type a message.");
      return;
    }

    // Store chat context in localStorage
    localStorage.setItem("chat.agent", agentName);
    localStorage.setItem("chat.message", message);

    // Redirect to messages.html
    window.location.href = "messages.html";
  }
 

