let horsesController; // Array to store horse elements
const horses = []; // Array to store horse elements
const speed = 2; // Speed of movement
let horseCount;
let isLoged; // Variable to store login status

document.addEventListener("DOMContentLoaded", function () {
    const data = GetData(); // Call GetData to fetch the JSON response
    horsesController = data.Horses; // Extract the list of horses
    isLoged = data.IsLoged; // Extract the login status
    horseCount = horsesController.length; // Get the number of horses
    spawnHorses();
    moveHorses();
});

function GetData() {
    let result;
    $.ajax({ 
        url: '/Horse/GetAllHorses',
        async: false, // Synchronous request (not recommended for production)
        dataType: "json",
        success: (givenData) => {
            result = givenData; // Store the parsed JSON response
        },
    });
    return result; // Return the parsed JSON object
}

// Function to create and spawn horses
function spawnHorses() {
    const horseContainer = document.querySelector('.horse-container');

    // Tooltip creation
    const tooltip = document.createElement('div');
    tooltip.className = 'tooltip';
    horseContainer.appendChild(tooltip); // Append the tooltip to horse container

    for (let i = 0; i < horseCount; i++) {
        const horse = document.createElement('img');
        horse.src = horsesController[i].Breed.Url; // Correct path to horse image
        horse.className = 'horse';
        horse.style.left = `${Math.random() * horseContainer.offsetWidth}px`;
        horse.style.top = `${Math.random() * horseContainer.offsetHeight}px`;
        horse.style.width = '100px';
        horse.style.height = '100px';
        horseContainer.appendChild(horse); // Add horse to the container

        // Add to array with initial properties
        horses.push({
            element: horse,
            x: parseFloat(horse.style.left),
            y: parseFloat(horse.style.top),
            directionX: Math.random() < 0.5 ? -1 : 1,
            directionY: Math.random() < 0.5 ? -1 : 1,
            isResting: false, // Flag to determine if horse is resting
            isHovered: false,
            rotationAngle: 0, // Current tilt angle for rotation animation
            noddingDirection: 1, // Direction of the nodding (1 for forward, -1 for backward)
            info: horsesController[i]
        });

        // Add hover event listeners
        horse.addEventListener('mouseover', (event) => {
            horses[i].isHovered = true; // Stop movement
            tooltip.style.display = 'block';
            tooltip.innerHTML = `
                <strong>Номер:</strong> ${horses[i].info.Number} <br>
                <strong>Порода:</strong> ${horses[i].info.Breed.Name} <br>
                <strong>Пол:</strong> ${horses[i].info.Gender} <br>
                <strong>Цена за час:</strong> ${horses[i].info.Price} <br>
                <strong>Височина:</strong> ${horses[i].info.Height} <br>
                <strong>Година на раждане:</strong> ${horses[i].info.BirhtYear.toString().substring(0, 10)}
            `;

            // Update tooltip position near mouse cursor
            updateTooltipPosition(event, horseContainer, tooltip);
        });

        horse.addEventListener('mousemove', (event) => {
            if (horses[i].isHovered) {
                updateTooltipPosition(event, horseContainer, tooltip);
            }
        });

        horse.addEventListener('mouseout', () => {
            horses[i].isHovered = false; // Resume movement
            tooltip.style.display = 'none';
        });

        horse.addEventListener('click', () => {
            const horseId = horses[i].info.Id;
            if (isLoged){ 
                window.location.href = `/Horse/Details/${horseId}`;
            }
        });
    }
}

function updateTooltipPosition(event, container, tooltip) {
    const containerRect = container.getBoundingClientRect();
    const mouseX = event.clientX - containerRect.left;
    const mouseY = event.clientY - containerRect.top;
    tooltip.style.left = `${mouseX + 10}px`; // Slightly to the right of the cursor
    tooltip.style.top = `${mouseY + 10}px`; // Slightly below the cursor
}

// Function to move all horses
function moveHorses() {
    const horseContainer = document.querySelector('.horse-container');

    horses.forEach((horseData) => {
        const horse = horseData.element;

        // Handle resting behavior
        if (horseData.isResting) {
            if (Math.random() < 0.01) {
                horseData.isResting = false;
                horse.style.transition = 'transform 0.2s'; // Reset for smooth motion
            }
            return; // Skip movement while resting
        }

        if (Math.random() < 0.005 || horseData.isHovered) {
            horseData.isResting = true;
            horse.style.transition = 'transform 1s'; // Smooth transition to rest
            horse.style.transform = 'rotate(0deg)'; // Reset rotation during rest
            return;
        }

        // Randomly change direction
        if (Math.random() < 0.01) {
            horseData.directionX = Math.random() < 0.5 ? -1 : 1;
            horseData.directionY = Math.random() < 0.5 ? -1 : 1;
        }

        // Update position
        horseData.x += horseData.directionX * speed;
        horseData.y += horseData.directionY * speed;

        // Handle container boundaries
        if (horseData.x < 0) {
            horseData.x = 0;
            horseData.directionX = 1;
        } else if (horseData.x + horse.offsetWidth > horseContainer.offsetWidth) {
            horseData.x = horseContainer.offsetWidth - horse.offsetWidth;
            horseData.directionX = -1;
        }

        if (horseData.y < 0) {
            horseData.y = 0;
            horseData.directionY = 1;
        } else if (horseData.y + horse.offsetHeight > horseContainer.offsetHeight) {
            horseData.y = horseContainer.offsetHeight - horse.offsetHeight;
            horseData.directionY = -1;
        }

        // Nodding effect (rotate back and forth)
        if (horseData.noddingDirection === 1) {
            horseData.rotationAngle += 1; // Rotate in the forward direction
            if (horseData.rotationAngle >= 10) {
                horseData.noddingDirection = -1; // Reverse direction when max angle is reached
            }
        } else {
            horseData.rotationAngle -= 1; // Rotate in the backward direction
            if (horseData.rotationAngle <= -10) {
                horseData.noddingDirection = 1; // Reverse direction when min angle is reached
            }
        }

        // Apply position and rotation
        horse.style.left = `${horseData.x}px`;
        horse.style.top = `${horseData.y}px`;
        horse.style.transform = horseData.directionX === -1
            ? `scaleX(-1) rotate(${horseData.rotationAngle}deg)`
            : `scaleX(1) rotate(${horseData.rotationAngle}deg)`;
    });

    // Continue animation
    requestAnimationFrame(moveHorses);
}